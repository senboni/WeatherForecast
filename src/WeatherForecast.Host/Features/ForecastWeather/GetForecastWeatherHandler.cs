using CSharpFunctionalExtensions;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.ForecastWeather;

public static class GetForecastWeatherHandler
{
    public static async Task<IResult<GetForecastWeatherResponse, Error>> Handle(
        GetForecastWeatherRequest request,
        IWeatherProvider weatherProvider,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.City))
        {
            return Result.Failure<GetForecastWeatherResponse, Error>(
                Error.UserError("City parameter must not be empty."));
        }

        using var forecastResponse = await weatherProvider.GetForecastWeather(request.City);

        if (!forecastResponse.IsSuccessStatusCode)
        {
            return Result.Failure<GetForecastWeatherResponse, Error>(new Error(
                message: $"Weather provider's response code ({forecastResponse.StatusCode}) does not indicate success.",
                statusCode: forecastResponse.StatusCode));
        }

        using var stream = await forecastResponse.Content.ReadAsStreamAsync(cancellationToken);
        var forecastObject = await JsonSerializer.DeserializeAsync<ForecastObject>(stream, cancellationToken: cancellationToken);

        if (forecastObject is null)
        {
            return Result.Failure<GetForecastWeatherResponse, Error>(
                Error.ServerError("Failed deserializing response content."));
        }

        var result = request.DateTime is null
            ? allForecasts()
            : forecastForDate((DateTime)request.DateTime);

        return Result.Success<GetForecastWeatherResponse, Error>(result);

        GetForecastWeatherResponse allForecasts()
            => new()
            {
                City = forecastObject.city.name,
                Country = forecastObject.city.country,
                Forecasts = forecastObject.list.Select(x =>
                {
                    var weather = x.weather.FirstOrDefault();
                    var descriptionParts = new string?[] { weather?.main, weather?.description };

                    return new GetForecastWeatherResponse.Forecast
                    {
                        DateTime = x.dt.ToDateTime(),
                        Description = string.Join(", ", descriptionParts.Where(x => !string.IsNullOrEmpty(x))),
                        Temperature = x.main.temp,
                        WindSpeed = x.wind.speed,
                    };
                }),
            };

        GetForecastWeatherResponse forecastForDate(DateTime date)
        {
            var closestMatch = findClosest(forecastObject.list, date);
            var weather = closestMatch.weather.FirstOrDefault();
            var descriptionParts = new string?[] { weather?.main, weather?.description };

            return new GetForecastWeatherResponse
            {
                City = forecastObject.city.name,
                Country = forecastObject.city.country,
                Forecasts = [new GetForecastWeatherResponse.Forecast
                {
                    DateTime = closestMatch.dt.ToDateTime(),
                    Description = string.Join(", ", descriptionParts.Where(x => !string.IsNullOrEmpty(x))),
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
    public int dt { get; set; }
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