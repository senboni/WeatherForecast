using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.IntegrationTests.Mocks;

public class BrokenWeatherProviderMock : IWeatherProvider
{
    public Task<HttpResponseMessage> GetCurrentWeather(string city, string unit)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> GetForecastWeather(string city, string unit)
    {
        throw new NotImplementedException();
    }
}
