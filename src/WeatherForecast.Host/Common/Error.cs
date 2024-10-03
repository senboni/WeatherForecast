using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace WeatherForecast.Host.Common;

public class Error
{
    public string[] Messages { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    public Error(IEnumerable<string>? messages = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Messages = messages is null ? [] : messages.ToArray();
        StatusCode = statusCode;
    }

    public Error(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Messages = [message];
        StatusCode = statusCode;
    }

    public static Error UserError(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        if ((int)statusCode < 400 || (int)statusCode > 499)
        {
            throw new ArgumentException($"'{nameof(statusCode)}' must be a user error status code (400-500).");
        }

        return new(message, statusCode);
    }

    public static Error ServerError(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        if ((int)statusCode < 500 || (int)statusCode > 599)
        {
            throw new ArgumentException($"'{nameof(statusCode)}' must be a server error status code (500-600).");
        }

        return new(message, statusCode);
    }

    public static Error WeatherProviderError(HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        => new("Unable to receive response from weather provider.", statusCode);
}
