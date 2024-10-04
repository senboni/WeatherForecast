using CSharpFunctionalExtensions;
using System.Net;
using System.Text.Json;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.IntegrationTests.Mocks;

public class OpenWeatherMapMock : IWeatherProvider
{
    public async Task<Result<TValue, HttpStatusCode>> GetCurrentWeather<TValue>(string city, TemperatureUnit unit, CancellationToken cancellationToken)
    {
        var (testFile, statusCode) = city.ToLower() switch
        {
            "london" => (Path.Combine("TestFiles", "currentweather_london-success.json"), HttpStatusCode.OK),
            "" => (Path.Combine("TestFiles", "city_not_found.json"), HttpStatusCode.BadRequest),
            _ => (Path.Combine("TestFiles", "nothing_to_geocode.json"), HttpStatusCode.NotFound),
        };

        using var stream = File.OpenRead(testFile);
        var value = await JsonSerializer.DeserializeAsync<TValue>(stream, cancellationToken: cancellationToken);

        if (value is null || !IsSuccessStatusCode(statusCode))
        {
            return Result.Failure<TValue, HttpStatusCode>(statusCode);
        }

        return Result.Success<TValue, HttpStatusCode>(value);
    }

    public async Task<Result<TValue, HttpStatusCode>> GetForecastWeather<TValue>(string city, TemperatureUnit unit, CancellationToken cancellationToken)
    {
        var (testFile, statusCode) = city.ToLower() switch
        {
            "london" => (Path.Combine("TestFiles", "forecastweather_london-success.json"), HttpStatusCode.OK),
            "" => (Path.Combine("TestFiles", "city_not_found.json"), HttpStatusCode.BadRequest),
            _ => (Path.Combine("TestFiles", "nothing_to_geocode.json"), HttpStatusCode.NotFound),
        };

        using var stream = File.OpenRead(testFile);
        var value = await JsonSerializer.DeserializeAsync<TValue>(stream, cancellationToken: cancellationToken);

        if (value is null || !IsSuccessStatusCode(statusCode))
        {
            return Result.Failure<TValue, HttpStatusCode>(statusCode);
        }

        return Result.Success<TValue, HttpStatusCode>(value);
    }

    private static bool IsSuccessStatusCode(HttpStatusCode statusCode)
        => (int)statusCode >= 200 && (int)statusCode < 300;
}
