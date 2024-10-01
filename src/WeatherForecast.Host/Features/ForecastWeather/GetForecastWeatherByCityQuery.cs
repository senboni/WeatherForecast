using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Features.CurrentWeather;

namespace WeatherForecast.Host.Features.ForecastWeather;

public static class GetForecastWeather
{
    public static async Task<IResult> ByCityEndpoint([FromQuery(Name = "city")] string city, IMediator mediator)
    {
        var result = await mediator.Send(new GetForecastWeatherByCityQuery(city));

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest();
    }
}

public record GetForecastWeatherByCityQuery(string City) : IRequest<ApiResponse<object>>;

public class GetForecastWeatherByCityValidator : AbstractValidator<GetCurrentWeatherByCityQuery>
{
    public GetForecastWeatherByCityValidator()
    {
        RuleFor(x => x.City).NotEmpty();
    }
}
