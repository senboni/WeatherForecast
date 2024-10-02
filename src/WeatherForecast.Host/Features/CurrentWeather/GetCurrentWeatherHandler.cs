using MediatR;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.CurrentWeather;

public class GetCurrentWeatherHandler(IWeatherProvider weatherProvider)
    : IRequestHandler<GetCurrentWeatherRequest, ApiResponse<GetCurrentWeatherResponse>>
{
    private readonly IWeatherProvider _weatherProvider = weatherProvider;

    public async Task<ApiResponse<GetCurrentWeatherResponse>> Handle(GetCurrentWeatherRequest request, CancellationToken cancellationToken)
    {
        using var httpResponseMessage = await _weatherProvider.GetCurrentWeather(request.City);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return ApiResponse<GetCurrentWeatherResponse>.Failure(
                message: $"Weather provider's response code ({httpResponseMessage.StatusCode}) does not indicate success.",
                statusCode: httpResponseMessage.StatusCode);
        }

        using var stream = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);
        var currentWeatherObject = await JsonSerializer.DeserializeAsync<CurrentWeatherResponse>(stream, cancellationToken: cancellationToken);

        if (currentWeatherObject is null)
        {
            return ApiResponse<GetCurrentWeatherResponse>.Failure(
                message: "Failed deserializing response content.",
                statusCode: HttpStatusCode.InternalServerError);
        }

        var weather = currentWeatherObject.weather.FirstOrDefault();
        var descriptionParts = new string?[] { weather?.main, weather?.description };

        return ApiResponse<GetCurrentWeatherResponse>.Success(new GetCurrentWeatherResponse
        {
            City = currentWeatherObject.name,
            Country = currentWeatherObject.sys.country,
            DateTime = currentWeatherObject.dt.ToDateTime(),
            Description = string.Join(", ", descriptionParts.Where(x => !string.IsNullOrEmpty(x))),
            Temperature = currentWeatherObject.main.temp,
            WindSpeed = currentWeatherObject.wind.speed,
        });
    }
}

#pragma warning disable CS8618
#pragma warning disable IDE1006
file class CurrentWeatherResponse
{
    public Weather[] weather { get; set; }
    public Main main { get; set; }
    public Wind wind { get; set; }
    public int dt { get; set; }
    public Sys sys { get; set; }
    public int timezone { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public int cod { get; set; }
}

file class Main
{
    public float temp { get; set; }
    public float feels_like { get; set; }
    public float temp_min { get; set; }
    public float temp_max { get; set; }
    public int pressure { get; set; }
    public int humidity { get; set; }
    public int sea_level { get; set; }
    public int grnd_level { get; set; }
}

file class Wind
{
    public float speed { get; set; }
    public int deg { get; set; }
    public float gust { get; set; }
}

file class Sys
{
    public int type { get; set; }
    public int id { get; set; }
    public string country { get; set; }
    public int sunrise { get; set; }
    public int sunset { get; set; }
}

file class Weather
{
    public int id { get; set; }
    public string main { get; set; }
    public string description { get; set; }
    public string icon { get; set; }
}
#pragma warning restore IDE1006
#pragma warning restore CS8618