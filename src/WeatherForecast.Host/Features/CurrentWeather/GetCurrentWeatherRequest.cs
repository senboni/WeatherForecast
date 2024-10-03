using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.CurrentWeather;

public record GetCurrentWeatherRequest(string City)
{
    public static async Task<IResult> Endpoint(
        IWeatherProvider weatherProvider,
        [FromQuery(Name = "city")] string city,
        CancellationToken cancellationToken)
    {
        var result = await GetCurrentWeatherHandler.Handle(
            new GetCurrentWeatherRequest(city),
            weatherProvider,
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.Error.ToProblemResult();
    }
}

public class GetCurrentWeatherResponse
{
    public required string Description { get; init; }
    public required double Temperature { get; init; }
    public required double WindSpeed { get; init; }
    public required DateTime DateTime { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
}
