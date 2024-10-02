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
        var result = await mediator.Send(new GetCurrentWeatherRequest(city));

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.StatusCode((int)result.StatusCode);
    }
}

public record GetCurrentWeatherRequest(string City) : IRequest<ApiResponse<GetCurrentWeatherResponse>>;

public class GetCurrentWeatherByCityValidator : AbstractValidator<GetForecastWeatherRequest>
{
    public GetCurrentWeatherByCityValidator()
    {
        RuleFor(x => x.City).NotEmpty();
    }
}
