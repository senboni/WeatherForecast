using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Features.ForecastWeather;

namespace WeatherForecast.Host.Features.CurrentWeather;

public static class GetCurrentWeather
{
    public static async Task<IResult> ByCityEndpoint([FromQuery(Name = "city")] string city, IMediator mediator)
    {
        var result = await mediator.Send(new GetCurrentWeatherByCityQuery(city));

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest();
    }
}

public record GetCurrentWeatherByCityQuery(string City) : IRequest<ApiResponse<object>>;

public class GetCurrentWeatherByCityValidator : AbstractValidator<GetForecastWeatherByCityQuery>
{
    public GetCurrentWeatherByCityValidator()
    {
        RuleFor(x => x.City).NotEmpty();
    }
}
