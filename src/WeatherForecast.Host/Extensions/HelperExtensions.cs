using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using WeatherForecast.Host.Common;

namespace WeatherForecast.Host.Extensions;

public static class HelperExtensions
{
    public static DateTime ToDateTime(this int unixTimestamp)
        => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp);

    public static IResult ToProblem(this ApiResponse response)
        => Results.Problem(
            title: response.StatusCode.ToString(),
            statusCode: (int)response.StatusCode,
            detail: response.Message);

    public static IResult ToProblemResult<TValue>(this ApiResponse<TValue> response) where TValue : class
    {
        if (response.Value is null)
        {
            return Results.Problem(
                title: response.StatusCode.ToString(),
                statusCode: (int)response.StatusCode,
                detail: response.Message);
        }

        return Results.Problem(
            title: response.StatusCode.ToString(),
            statusCode: (int)response.StatusCode,
            detail: response.Message,
            extensions: new Dictionary<string, object?>
            {
                { "error", response.Value }
            });
    }
}
