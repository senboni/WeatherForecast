using WeatherForecast.Host.Common;

namespace WeatherForecast.Host.WeatherProviders;

public class OpenWeatherMapWeatherProvider(IHttpClientFactory httpClientFactory) : IWeatherProvider
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task GetCurrentWeather(string city)
    {
        var client = _httpClientFactory.CreateClient(Constants.OpenWeatherMapClient);
        var response = await client.GetAsync($"/weather?q={city}");
    }
}