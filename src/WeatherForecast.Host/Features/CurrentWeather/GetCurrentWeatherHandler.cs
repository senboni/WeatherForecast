using CSharpFunctionalExtensions;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.CurrentWeather;

public static class GetCurrentWeatherHandler
{
    public static async Task<IResult<GetCurrentWeather.Response, Error>> Handle(
        GetCurrentWeather.Request request,
        IWeatherProvider weatherProvider,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.City))
        {
            return Result.Failure<GetCurrentWeather.Response, Error>(
                Error.UserError("City parameter must not be empty."));
        }

        if (Constants.TemperatureUnits.All(x => x != request.Unit))
        {
            return Result.Failure<GetCurrentWeather.Response, Error>(
                Error.UserError("Invalid temperature unit. Available units: c (celsius), f (fahrenheit), k (kelvin)."));
        }

        var result = await weatherProvider.GetCurrentWeather<CurrentWeatherObject>(request.City, request.Unit, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error switch
            {
                HttpStatusCode.NotFound => Result.Failure<GetCurrentWeather.Response, Error>(Error.UserError("Unable to find city.", HttpStatusCode.NotFound)),
                _ => Result.Failure<GetCurrentWeather.Response, Error>(Error.WeatherProviderError()),
            };
        }

        var currentWeatherObject = result.Value;
        var weather = currentWeatherObject.weather.FirstOrDefault();
        var description = Make.WeatherDescription(currentWeatherObject.main.temp, request.Unit, weather?.main, weather?.description);

        return Result.Success<GetCurrentWeather.Response, Error>(new GetCurrentWeather.Response
        {
            City = currentWeatherObject.name,
            Country = currentWeatherObject.sys.country,
            DateTime = currentWeatherObject.dt.ToDateTime(),
            Description = description,
            Temperature = currentWeatherObject.main.temp,
            WindSpeed = currentWeatherObject.wind.speed,
        });
    }
}

#pragma warning disable CS8618
#pragma warning disable IDE1006
file class CurrentWeatherObject
{
    public Weather[] weather { get; set; }
    public Main main { get; set; }
    public Wind wind { get; set; }
    public double dt { get; set; }
    public Sys sys { get; set; }
    public string name { get; set; }
}

file class Main
{
    public double temp { get; set; }
}

file class Wind
{
    public double speed { get; set; }
}

file class Sys
{
    public string country { get; set; }
}

file class Weather
{
    public string main { get; set; }
    public string description { get; set; }
}
#pragma warning restore IDE1006
#pragma warning restore CS8618