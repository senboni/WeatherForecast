using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Features.CurrentWeather;

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
            : Results.StatusCode((int)result.StatusCode);
    }
}

public record GetForecastWeatherRequest(string City, DateTime? DateTime = null) : IRequest<ApiResponse<GetForecastWeatherResponse>>;

public class GetForecastWeatherByCityValidator : AbstractValidator<GetCurrentWeatherRequest>
{
    public GetForecastWeatherByCityValidator()
    {
        RuleFor(x => x.City).NotEmpty();
    }
}
