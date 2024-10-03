﻿using System.Net;
using WeatherForecast.Host.Common;

namespace WeatherForecast.UnitTests.Common;

public class ErrorTests
{
    [Fact]
    public void ErrorConstructor_ValidationTests()
    {
        //arrange
        string[] messages = ["msg1", "msg2"];
        var statusCode = HttpStatusCode.BadRequest;

        //act
        var actual = new Error(messages, statusCode);

        //assert
        Assert.Equal(messages, actual.Messages);
        Assert.Equal(statusCode, actual.StatusCode);
    }

    [Fact]
    public void ErrorSingleMessageConstructor_ValidationTests()
    {
        //arrange
        var message = "msg1";
        var statusCode = HttpStatusCode.OK;

        //act
        var actual = new Error(message, statusCode);

        //assert
        Assert.Single(actual.Messages);
        Assert.Equal(message, actual.Messages[0]);
        Assert.Equal(statusCode, actual.StatusCode);
    }

    [Fact]
    public void UserError_MustReturn_StatusCode400()
    {
        //arrange
        var message = "msg1";
        var expectedStatusCode = HttpStatusCode.BadRequest;

        //act
        var actual = Error.UserError(message);

        //assert
        Assert.Single(actual.Messages);
        Assert.Equal(message, actual.Messages[0]);
        Assert.Equal(expectedStatusCode, actual.StatusCode);
    }

    [Fact]
    public void ServerError_MustReturn_StatusCode500()
    {
        //arrange
        var message = "msg1";
        var expectedStatusCode = HttpStatusCode.InternalServerError;

        //act
        var actual = Error.ServerError(message);

        //assert
        Assert.Single(actual.Messages);
        Assert.Equal(message, actual.Messages[0]);
        Assert.Equal(expectedStatusCode, actual.StatusCode);
    }
}