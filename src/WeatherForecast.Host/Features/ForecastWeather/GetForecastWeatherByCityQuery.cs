using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.ForecastWeather;

public static partial class GetForecastWeather
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

public class GetForecastWeatherByCityHandler(IWeatherProvider weatherProvider) : IRequestHandler<GetForecastWeatherByCityQuery, ApiResponse<object>>
{
    private readonly IWeatherProvider _weatherProvider = weatherProvider;

    public async Task<ApiResponse<object>> Handle(GetForecastWeatherByCityQuery request, CancellationToken cancellationToken)
    {
        var response = await _weatherProvider.GetForecastWeather(request.City);

        return response.IsSuccessStatusCode
            ? ApiResponse<object>.Success(new { Content = await response.Content.ReadAsStringAsync(cancellationToken) })
            : ApiResponse<object>.Failure(message: $"Weather provider's response code ({response.StatusCode}) does not indicate success.");
    }
}
