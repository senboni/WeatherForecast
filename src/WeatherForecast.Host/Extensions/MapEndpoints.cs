using WeatherForecast.Host.Features.CurrentWeather;

namespace WeatherForecast.Host.Extensions;

public static class MapEndpoints
{
    public static WebApplication MapOpenWeatherMapEndpoints(this WebApplication group)
    {
        group.MapGet("/currentweather", GetCurrentWeather.ByCity)
            .WithName("GetCurrentWeather")
            .WithOpenApi();

        return group;
    }
}
