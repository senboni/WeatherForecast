using System.Net;
using System.Text.Json;
using WeatherForecast.Host.Features.CurrentWeather;

namespace WeatherForecast.IntegrationTests.Features;

public class GetCurrentWeatherTests(ApiFixture fixture) : IntegrationTestBase(fixture)
{
    private static readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task GetCurrentWeather_GivenCity_ShouldReturnCurrentWeather()
    {
        //arrange
        var expectedModel = new GetCurrentWeather.Response
        {
            City = "London",
            Country = "GB",
            DateTime = new DateTime(2024, 10, 3, 10, 27, 37),
            Description = "Clouds, broken clouds",
            Temperature = 286.2,
            WindSpeed = 0.45,
        };

        //act
        var response = await ApiFixture.HttpClient.GetAsync("/currentweather?city=London");

        //assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var stream = await response.Content.ReadAsStreamAsync();
        var actualModel = await JsonSerializer.DeserializeAsync<GetCurrentWeather.Response>(stream, _serializerOptions);

        Assert.Equal(expectedModel, actualModel);
    }

    [Fact]
    public async Task GetCurrentWeather_WithoutSpecifiedCity_ShouldReturnBadRequest()
    {
        //act
        var response = await ApiFixture.HttpClient.GetAsync("/currentweather?city=");

        //assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCurrentWeather_GivenInvalidCity_ShouldReturnNotFound()
    {
        //act
        var response = await ApiFixture.HttpClient.GetAsync("/currentweather?city=asdasdsa");

        //assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
