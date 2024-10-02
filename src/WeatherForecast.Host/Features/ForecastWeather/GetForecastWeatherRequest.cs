using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Extensions;

namespace WeatherForecast.Host.Features.ForecastWeather;

public static class GetForecastWeather
{
    public static async Task<IResult> ByCityEndpoint(
        IMediator mediator,
        [FromQuery(Name = "city")] string city,
        [FromQuery(Name = "date")] DateTime? dateTime = null)
    {
        var result = await mediator.Send(new GetForecastWeatherRequest(city, dateTime));

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult();
    }
}

public record GetForecastWeatherRequest(string City, DateTime? DateTime = null) : IRequest<ApiResponse<GetForecastWeatherResponse>>;

public class GetForecastWeatherByCityValidator : AbstractValidator<GetForecastWeatherRequest>
{
    public GetForecastWeatherByCityValidator()
    {
        RuleFor(x => x.City).NotEmpty();
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
