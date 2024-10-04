using CSharpFunctionalExtensions;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.ForecastWeather;

public static class GetForecastWeatherHandler
{
    public static async ValueTask<IResult<GetForecastWeather.Response, Error>> Handle(
        GetForecastWeather.Request request,
        IWeatherProvider weatherProvider,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.City))
        {
            return Result.Failure<GetForecastWeather.Response, Error>(
                Error.UserError("City parameter must not be empty."));
        }

        if (!Enum.TryParse(request.Unit, true, out TemperatureUnit unit))
        {
            return Result.Failure<GetForecastWeather.Response, Error>(
                Error.UserError("Invalid temperature unit. Available units: c (celsius), f (fahrenheit), k (kelvin)."));
        }

        var result = await weatherProvider.GetForecastWeather<ForecastObject>(request.City, unit, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error switch
            {
                HttpStatusCode.NotFound => Result.Failure<GetForecastWeather.Response, Error>(Error.UserError("Unable to find city.", HttpStatusCode.NotFound)),
                _ => Result.Failure<GetForecastWeather.Response, Error>(Error.WeatherProviderError()),
            };
        }

        var forecastObject = result.Value;

        var response = request.DateTime is null
            ? allForecasts()
            : forecastForDate((DateTime)request.DateTime);

        return Result.Success<GetForecastWeather.Response, Error>(response);

        GetForecastWeather.Response allForecasts()
            => new()
            {
                City = forecastObject.city.name,
                Country = forecastObject.city.country,
                Forecasts = forecastObject.list.Select(x =>
                {
                    var weather = x.weather.FirstOrDefault();

                    return new GetForecastWeather.Response.Forecast
                    {
                        DateTime = x.dt.ToDateTime(),
                        Description = Make.WeatherDescription(x.main.temp, request.Unit, weather?.main, weather?.description),
                        Temperature = x.main.temp,
                        WindSpeed = x.wind.speed,
                    };
                })
                .ToArray(),
            };

        GetForecastWeather.Response forecastForDate(DateTime date)
        {
            var closestMatch = findClosest(forecastObject.list, date);
            var weather = closestMatch.weather.FirstOrDefault();

            return new GetForecastWeather.Response
            {
                City = forecastObject.city.name,
                Country = forecastObject.city.country,
                Forecasts = [new GetForecastWeather.Response.Forecast
                {
                    DateTime = closestMatch.dt.ToDateTime(),
                    Description = Make.WeatherDescription(closestMatch.main.temp, request.Unit, weather?.main, weather?.description),
                    Temperature = closestMatch.main.temp,
                    WindSpeed = closestMatch.wind.speed,
                }],
            };
        }

        Forecast findClosest(Forecast[] forecasts, DateTime targetDate)
            => forecasts
                .OrderBy(x => Math.Abs((x.dt.ToDateTime() - targetDate).Ticks))
                .First();
    }
}

#pragma warning disable CS8618
#pragma warning disable IDE1006
file class ForecastObject
{
    public Forecast[] list { get; set; }
    public City city { get; set; }
}

file class City
{
    public string name { get; set; }
    public string country { get; set; }
}

file class Forecast
{
    public double dt { get; set; }
    public Main main { get; set; }
    public Weather[] weather { get; set; }
    public Wind wind { get; set; }
}

file class Main
{
    public double temp { get; set; }
}

file class Wind
{
    public double speed { get; set; }
}

file class Weather
{
    public string main { get; set; }
    public string description { get; set; }
}
#pragma warning restore IDE1006
#pragma warning restore CS8618