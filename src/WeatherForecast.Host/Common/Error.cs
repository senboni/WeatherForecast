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

    public static Error UserError(string message)
        => new(message, HttpStatusCode.BadRequest);

    public static Error ServerError(string message)
        => new(message, HttpStatusCode.InternalServerError);
}
