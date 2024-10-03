using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WeatherForecast.Host.Common;

namespace WeatherForecast.Host.Extensions;

public static class HelperExtensions
{
    public static DateTime ToDateTime(this double unixTimestamp)
        => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp);

    public static IResult ToProblemResult(this Error error)
    {
        if (error.Messages.Length <= 1)
        {
            return Results.Problem(
                title: error.StatusCode.ToString(),
                statusCode: (int)error.StatusCode,
                detail: error.Messages[0]);
        }

        return Results.Problem(
            title: error.StatusCode.ToString(),
            statusCode: (int)error.StatusCode,
            detail: "Multiple errors occurred.",
            extensions: new Dictionary<string, object?>
            {
                { "errors", error.Messages },
            });
    }

    public static ProblemDetails ToProblemDetails(this IResult error)
        => error is ProblemHttpResult result
            ? result.ProblemDetails
            : new ProblemDetails();
}