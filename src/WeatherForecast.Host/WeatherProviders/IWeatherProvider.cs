
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherForecast.Host.WeatherProviders;

public interface IWeatherProvider
{
    Task<HttpResponseMessage> GetCurrentWeather(string city);
}
