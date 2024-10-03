using CSharpFunctionalExtensions;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.CurrentWeather;

public static class GetCurrentWeatherHandler
{
    public static async Task<IResult<GetCurrentWeatherResponse, Error>> Handle(
        GetCurrentWeatherRequest request,
        IWeatherProvider weatherProvider,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.City))
        {
            return Result.Failure<GetCurrentWeatherResponse, Error>(
                Error.UserError("City parameter must not be empty."));
        }

        using var currentWeatherResponse = await weatherProvider.GetCurrentWeather(request.City);

        if (!currentWeatherResponse.IsSuccessStatusCode)
        {
            return Result.Failure<GetCurrentWeatherResponse, Error>(new Error(
                message: $"Weather provider's response code ({currentWeatherResponse.StatusCode}) does not indicate success.",
                statusCode: currentWeatherResponse.StatusCode));
        }

        using var stream = await currentWeatherResponse.Content.ReadAsStreamAsync(cancellationToken);
        var currentWeatherObject = await JsonSerializer.DeserializeAsync<CurrentWeatherObject>(stream, cancellationToken: cancellationToken);

        if (currentWeatherObject is null)
        {
            return Result.Failure<GetCurrentWeatherResponse, Error>(
                Error.ServerError("Failed deserializing response content."));
        }

        var weather = currentWeatherObject.weather.FirstOrDefault();
        var descriptionParts = new string?[] { weather?.main, weather?.description };

        return Result.Success<GetCurrentWeatherResponse, Error>(new GetCurrentWeatherResponse
        {
            City = currentWeatherObject.name,
            Country = currentWeatherObject.sys.country,
            DateTime = currentWeatherObject.dt.ToDateTime(),
            Description = string.Join(", ", descriptionParts.Where(x => !string.IsNullOrEmpty(x))),
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
    public int dt { get; set; }
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