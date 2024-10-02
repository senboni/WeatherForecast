using MediatR;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.ForecastWeather;

public class GetForecastWeatherHandler(IWeatherProvider weatherProvider) : IRequestHandler<GetForecastWeatherRequest, ApiResponse<GetForecastWeatherResponse>>
{
    private readonly IWeatherProvider _weatherProvider = weatherProvider;

    public async Task<ApiResponse<GetForecastWeatherResponse>> Handle(GetForecastWeatherRequest request, CancellationToken cancellationToken)
    {
        using var forecastResponse = await _weatherProvider.GetForecastWeather(request.City);

        if (!forecastResponse.IsSuccessStatusCode)
        {
            return ApiResponse<GetForecastWeatherResponse>.Failure(
                message: $"Weather provider's response code ({forecastResponse.StatusCode}) does not indicate success.",
            statusCode: forecastResponse.StatusCode);
        }

        using var stream = await forecastResponse.Content.ReadAsStreamAsync(cancellationToken);
        var forecastObject = await JsonSerializer.DeserializeAsync<ForecastWeatherResponse>(stream, cancellationToken: cancellationToken);

        if (forecastObject is null)
        {
            return ApiResponse<GetForecastWeatherResponse>.Failure(
                message: "Failed deserializing response content.",
                statusCode: HttpStatusCode.InternalServerError);
        }

        return ApiResponse<GetForecastWeatherResponse>.Success(new GetForecastWeatherResponse
        {
            City = forecastObject.city.name,
            Country = forecastObject.city.country,
            Forecasts = forecastObject.list.Select(x =>
            {
                var weather = x.weather.FirstOrDefault();
                var descriptionParts = new string?[] { weather?.main, weather?.description };

                return new GetForecastWeatherResponse.Forecast
                {
                    DateTime = x.dt.ToDateTime(),
                    Description = string.Join(", ", descriptionParts.Where(x => !string.IsNullOrEmpty(x))),
                    Temperature = x.main.temp,
                    WindSpeed = x.wind.speed,
                };
            }),
        });
    }
}

#pragma warning disable CS8618
#pragma warning disable IDE1006
file class ForecastWeatherResponse
{
    public Forecast[] list { get; set; }
    public City city { get; set; }
}

file class City
{
    public string name { get; set; }
    public string country { get; set; }
}

file class Forecast
{
    public int dt { get; set; }
    public Main main { get; set; }
    public Weather[] weather { get; set; }
    public Wind wind { get; set; }
}

file class Main
{
    public double temp { get; set; }
}

file class Wind
{
    public double speed { get; set; }
}

file class Weather
{
    public string main { get; set; }
    public string description { get; set; }
}
#pragma warning restore IDE1006
#pragma warning restore CS8618