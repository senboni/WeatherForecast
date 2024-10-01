using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.ForecastWeather;

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
