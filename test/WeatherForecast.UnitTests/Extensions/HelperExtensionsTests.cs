using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.Extensions;

namespace WeatherForecast.UnitTests.Extensions;

public class HelperExtensionsTests
{
    [Fact]
    public void ToDateTime_OneBillionthSecondTimestamp_ShouldReturn2001Date()
    {
        //arrange
        double unix = 1_000_000_000;
        var expected = new DateTime(2001, 9, 9, 1, 46, 40);

        //act
        var actual = unix.ToDateTime();

        //assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToProblemResult_GivenError_ShouldCreateCorrespondingResult()
    {
        //arrange
        var expectedDetail = "Something went wrong";
        var expectedStatus = HttpStatusCode.InternalServerError;
        var expectedTitle = expectedStatus.ToString();
        var error = new Error(expectedDetail, HttpStatusCode.InternalServerError);

        //act
        var actual = (ProblemHttpResult)error.ToProblemResult();

        //assert
        Assert.Equal(expectedDetail, actual.ProblemDetails.Detail);
        Assert.Equal((int)expectedStatus, actual.ProblemDetails.Status);
        Assert.Equal(expectedTitle, actual.ProblemDetails.Title);
    }

    [Fact]
    public void ToProblemResult_GivenMultipleErrors_ShouldCreateResultWithExtensions()
    {
        //arrange
        var expectedDetail = "Multiple errors occurred.";
        var expectedKey = "errors";
        string[] expectedErrors = ["error1", "error2"];
        var expectedStatus = HttpStatusCode.BadRequest;
        var expectedTitle = expectedStatus.ToString();
        var error = new Error(expectedErrors, expectedStatus);

        //act
        var actual = (ProblemHttpResult)error.ToProblemResult();

        //assert
        Assert.Equal(expectedDetail, actual.ProblemDetails.Detail);
        var extension = actual.ProblemDetails.Extensions.First();
        Assert.Equal(expectedKey, extension.Key);
        Assert.Equal(expectedErrors, extension.Value);
        Assert.Equal((int)expectedStatus, actual.ProblemDetails.Status);
        Assert.Equal(expectedTitle, actual.ProblemDetails.Title);
    }
}
