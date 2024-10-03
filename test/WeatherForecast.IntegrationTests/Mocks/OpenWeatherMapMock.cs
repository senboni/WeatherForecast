using System.Net;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.IntegrationTests.Mocks;

public class OpenWeatherMapMock : IWeatherProvider
{
    public async Task<HttpResponseMessage> GetCurrentWeather(string city, string unit)
    {
        var (testFile, statusCode) = city.ToLower() switch
        {
            "london" => (Path.Combine("TestFiles", "currentweather_london-success.json"), HttpStatusCode.OK),
            "" => (Path.Combine("TestFiles", "city_not_found.json"), HttpStatusCode.BadRequest),
            _ => (Path.Combine("TestFiles", "nothing_to_geocode.json"), HttpStatusCode.NotFound),
        };

        var content = await File.ReadAllTextAsync(testFile);

        return new HttpResponseMessage
        {
            Content = new StringContent(content),
            StatusCode = statusCode,
        };
    }

    public async Task<HttpResponseMessage> GetForecastWeather(string city, string unit)
    {
        var (testFile, statusCode) = city.ToLower() switch
        {
            "london" => (Path.Combine("TestFiles", "forecastweather_london-success.json"), HttpStatusCode.OK),
            "" => (Path.Combine("TestFiles", "city_not_found.json"), HttpStatusCode.BadRequest),
            _ => (Path.Combine("TestFiles", "nothing_to_geocode.json"), HttpStatusCode.NotFound),
        };

        var content = await File.ReadAllTextAsync(testFile);

        return new HttpResponseMessage
        {
            Content = new StringContent(content),
            StatusCode = statusCode,
        };
    }
}
