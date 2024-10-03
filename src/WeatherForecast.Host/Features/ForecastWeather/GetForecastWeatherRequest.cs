using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.ForecastWeather;

public record GetForecastWeatherRequest(string City, DateTime? DateTime = null)
{
    public static async Task<IResult> Endpoint(
        IWeatherProvider weatherProvider,
        [FromQuery(Name = "city")] string city,
        [FromQuery(Name = "date")] DateTime? dateTime = null,
        CancellationToken cancellationToken = default)
    {
        var result = await GetForecastWeatherHandler.Handle(
            new GetForecastWeatherRequest(city, dateTime),
            weatherProvider,
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.Error.ToProblemResult();
    }
}

public class GetForecastWeatherResponse
{
    public required IEnumerable<Forecast> Forecasts { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }

    public class Forecast
    {
        public required string Description { get; init; }
        public required double Temperature { get; init; }
        public required double WindSpeed { get; init; }
        public required DateTime DateTime { get; init; }
    }
}
