using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.CurrentWeather;

public static partial class Endpoints
{
    public static async Task<IResult> GetCurrentWeatherByCityEndpoint([FromQuery(Name = "city")] string city, IMediator mediator)
    {
        var result = await mediator.Send(new GetCurrentWeatherByCityQuery(city));

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest();
    }
}

public record GetCurrentWeatherByCityQuery(string City) : IRequest<ApiResponse<object>>;

public class GetCurrentWeatherByCityHandler(IWeatherProvider weatherProvider) : IRequestHandler<GetCurrentWeatherByCityQuery, ApiResponse<object>>
{
    private readonly IWeatherProvider _weatherProvider = weatherProvider;

    public async Task<ApiResponse<object>> Handle(GetCurrentWeatherByCityQuery request, CancellationToken cancellationToken)
    {
        var response = await _weatherProvider.GetCurrentWeather(request.City);

        return response.IsSuccessStatusCode
            ? ApiResponse<object>.Success(new { Content = await response.Content.ReadAsStringAsync(cancellationToken) })
            : ApiResponse<object>.Failure(message: $"Weather provider's response code ({response.StatusCode}) does not indicate success.");
    }
}