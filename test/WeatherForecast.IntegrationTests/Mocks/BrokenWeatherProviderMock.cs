using CSharpFunctionalExtensions;
using System.Net;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.IntegrationTests.Mocks;

public class BrokenWeatherProviderMock : IWeatherProvider
{
    public Task<Result<TValue, HttpStatusCode>> GetCurrentWeather<TValue>(string city, TemperatureUnit unit, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TValue, HttpStatusCode>> GetForecastWeather<TValue>(string city, TemperatureUnit unit, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
