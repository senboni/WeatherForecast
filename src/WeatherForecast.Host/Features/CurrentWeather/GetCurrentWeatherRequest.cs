using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Extensions;

namespace WeatherForecast.Host.Features.CurrentWeather;

public static class GetCurrentWeather
{
    public static async Task<IResult> ByCityEndpoint([FromQuery(Name = "city")] string city, IMediator mediator)
    {
        var result = await mediator.Send(new GetCurrentWeatherRequest(city));

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult();
    }
}

public record GetCurrentWeatherRequest(string City) : IRequest<ApiResponse<GetCurrentWeatherResponse>>;

public class GetCurrentWeatherByCityValidator : AbstractValidator<GetCurrentWeatherRequest>
{
    public GetCurrentWeatherByCityValidator()
    {
        RuleFor(x => x.City).NotEmpty();
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