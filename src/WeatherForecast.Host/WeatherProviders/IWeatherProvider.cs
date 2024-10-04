using CSharpFunctionalExtensions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;

namespace WeatherForecast.Host.WeatherProviders;

public interface IWeatherProvider
{
    Task<Result<TValue, HttpStatusCode>> GetCurrentWeather<TValue>(string city, TemperatureUnit unit, CancellationToken cancellationToken);
    Task<Result<TValue, HttpStatusCode>> GetForecastWeather<TValue>(string city, TemperatureUnit unit, CancellationToken cancellationToken);
}
