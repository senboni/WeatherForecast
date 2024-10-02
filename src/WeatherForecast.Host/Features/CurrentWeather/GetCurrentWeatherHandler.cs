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
        using var currentWeatherResponse = await _weatherProvider.GetCurrentWeather(request.City);

        if (!currentWeatherResponse.IsSuccessStatusCode)
        {
            return ApiResponse<GetCurrentWeatherResponse>.Failure(
                message: $"Weather provider's response code ({currentWeatherResponse.StatusCode}) does not indicate success.",
                statusCode: currentWeatherResponse.StatusCode);
        }

        using var stream = await currentWeatherResponse.Content.ReadAsStreamAsync(cancellationToken);
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
    public string name { get; set; }
}

file class Main
{
    public double temp { get; set; }
}

file class Wind
{
    public double speed { get; set; }
}

file class Sys
{
    public string country { get; set; }
}

file class Weather
{
    public string main { get; set; }
    public string description { get; set; }
}
#pragma warning restore IDE1006
#pragma warning restore CS8618