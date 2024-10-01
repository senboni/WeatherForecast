
namespace WeatherForecast.Host.WeatherProviders;

public interface IWeatherProvider
{
    Task GetCurrentWeather(string city);
}
