using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Features.CurrentWeather;

public static class GetCurrentWeather
{
    public static async Task ByCity([FromQuery(Name = "city")] string city, IWeatherProvider weatherProvider)
    {
        await weatherProvider.GetCurrentWeather(city);
    }
}
