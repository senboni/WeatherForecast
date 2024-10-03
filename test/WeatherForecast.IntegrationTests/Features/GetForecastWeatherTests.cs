using System.Net;
using System.Text.Json;
using WeatherForecast.Host.Features.ForecastWeather;

namespace WeatherForecast.IntegrationTests.Features;

public class GetForecastWeatherTests(ApiFixture fixture) : IntegrationTestBase(fixture)
{
    private static readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task GetForecastWeather_GivenCity_ShouldReturnForecastWeather()
    {
        //arrange
        var expectedForecastsCount = 40;
        var expectedModel = new GetForecastWeather.Response
        {
            City = "Paris",
            Country = "FR",
            Forecasts = [new GetForecastWeather.Response.Forecast
            {
                DateTime = new DateTime(2024, 10, 3, 12, 0, 0),
                Description = "Clouds, overcast clouds, 285° K",
                Temperature = 285.32,
                WindSpeed = 4.43,
            }],
        };

        //act
        var response = await ApiFixture.HttpClient.GetAsync("/forecastweather?city=London&unit=k");

        //assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var stream = await response.Content.ReadAsStreamAsync();
        var actualModel = (await JsonSerializer.DeserializeAsync<GetForecastWeather.Response>(stream, _serializerOptions))!;

        Assert.Equal(expectedModel.City, actualModel.City);
        Assert.Equal(expectedModel.Country, actualModel.Country);
        Assert.Equal(expectedForecastsCount, actualModel.Forecasts.Length);
        Assert.Equal(expectedModel.Forecasts[0], actualModel.Forecasts[0]);
    }

    [Fact]
    public async Task GetForecastWeather_GivenCityAndDate_ShouldReturnForecastWeatherForDate()
    {
        //arrange
        var expectedForecastsCount = 1;
        var expectedModel = new GetForecastWeather.Response
        {
            City = "Paris",
            Country = "FR",
            Forecasts = [new GetForecastWeather.Response.Forecast
            {
                DateTime = new DateTime(2024, 10, 3, 12, 0, 0),
                Description = "Clouds, overcast clouds",
                Temperature = 285.31,
                WindSpeed = 4.43,
            }],
        };

        //act
        var response = await ApiFixture.HttpClient.GetAsync("/forecastweather?city=London&date=2024-10-3");

        //assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var stream = await response.Content.ReadAsStreamAsync();
        var actualModel = (await JsonSerializer.DeserializeAsync<GetForecastWeather.Response>(stream, _serializerOptions))!;

        Assert.Equal(expectedModel.City, actualModel.City);
        Assert.Equal(expectedModel.Country, actualModel.Country);
        Assert.Equal(expectedForecastsCount, actualModel.Forecasts.Length);
        Assert.Equal(actualModel.Forecasts[0], actualModel.Forecasts[0]);
    }

    [Fact]
    public async Task GetCurrentWeather_WithoutSpecifiedCity_ShouldReturnBadRequest()
    {
        //act
        var response = await ApiFixture.HttpClient.GetAsync("/forecastweather?city=");

        //assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCurrentWeather_GivenInvalidCity_ShouldReturnNotFound()
    {
        //act
        var response = await ApiFixture.HttpClient.GetAsync("/forecastweather?city=asdasdsa");

        //assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
