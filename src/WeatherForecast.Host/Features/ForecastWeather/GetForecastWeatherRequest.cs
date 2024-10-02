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
        var result = await mediator.Send(new GetForecastWeatherRequest(city));

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.StatusCode((int)result.StatusCode);
    }
}

public record GetForecastWeatherRequest(string City) : IRequest<ApiResponse<GetForecastWeatherResponse>>;

public class GetForecastWeatherByCityValidator : AbstractValidator<GetCurrentWeatherRequest>
{
    public GetForecastWeatherByCityValidator()
    {
        RuleFor(x => x.City).NotEmpty();
    }
}
