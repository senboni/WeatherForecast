using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.CurrentWeather;

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
