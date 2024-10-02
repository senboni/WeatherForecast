using System.Net;

namespace WeatherForecast.Host.Common;

public class ApiResponse
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    private ApiResponse(bool isSuccess, string message, HttpStatusCode statusCode)
    {
        IsSuccess = isSuccess;
        Message = message;
        StatusCode = statusCode;
    }

    public static ApiResponse Success(string message = "", HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(true, message, statusCode);

    public static ApiResponse Failure(string message = "", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(false, message, statusCode);
}

public class ApiResponse<TValue> where TValue : class
{
    public TValue? Value { get; init; }
    public bool IsSuccess { get; init; }
    public string Message { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    private ApiResponse(TValue? value, bool isSuccess, string message, HttpStatusCode statusCode)
    {
        Value = value;
        IsSuccess = isSuccess;
        Message = message;
        StatusCode = statusCode;
    }

    public static ApiResponse<TValue> Success(TValue value, string message = "", HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(value, true, message, statusCode);

    public static ApiResponse<TValue> Failure(TValue? value = null, string message = "", HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(value, false, message, statusCode);
}
