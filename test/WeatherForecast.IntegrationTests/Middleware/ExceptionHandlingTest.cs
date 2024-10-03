using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WeatherForecast.IntegrationTests.Middleware;

public class ExceptionHandlingTest(BrokenApiFixture brokenApiFixture) : IClassFixture<BrokenApiFixture>
{
    [Fact]
    public async Task ExceptionHandling()
    {
        //arrange
        var expectedDetail = "Something went wrong.";
        var expectedStatus = HttpStatusCode.InternalServerError;

        //act
        var response = await brokenApiFixture.HttpClient.GetAsync("/currentweather");

        //assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        var stream = await response.Content.ReadAsStreamAsync();
        var responseModel = await JsonSerializer.DeserializeAsync<ProblemDetails>(stream, Helpers.SerializerOptions);

        Assert.True(responseModel is not null);
        Assert.Equal(expectedDetail, responseModel.Detail);
        Assert.Equal(expectedStatus.ToString(), responseModel.Title);
        Assert.Equal((int)expectedStatus, responseModel.Status);
    }
}
