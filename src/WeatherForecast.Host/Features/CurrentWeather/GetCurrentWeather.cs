﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.CurrentWeather;

public static class GetCurrentWeather
{
    public static async Task<IResult> Endpoint(
        IWeatherProvider weatherProvider,
        [FromQuery(Name = "city")] string city,
        [FromQuery(Name = "unit")] string unit = "c",
        CancellationToken cancellationToken = default)
    {
        var request = new Request(city, unit.ToUpper());
        var result = await GetCurrentWeatherHandler.Handle(request, weatherProvider, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.Error.ToProblemResult();
    }

    public record Request(string City, string Unit);

    public record Response
    {
        public required string Description { get; init; }
        public required double Temperature { get; init; }
        public required double WindSpeed { get; init; }
        public required DateTime DateTime { get; init; }
        public required string City { get; init; }
        public required string Country { get; init; }
    }
}