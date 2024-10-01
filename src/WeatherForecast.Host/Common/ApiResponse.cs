namespace WeatherForecast.Host.Common;

public class ApiResponse
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; init; }

    private ApiResponse(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static ApiResponse Success(string message = "")
        => new(true, message);

    public static ApiResponse Failure(string message = "")
        => new(false, message);
}

public class ApiResponse<TValue> where TValue : class
{
    public TValue? Value { get; init; }
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; init; }

    private ApiResponse(TValue? value, bool isSuccess, string message)
    {
        Value = value;
        IsSuccess = isSuccess;
        Message = message;
    }

    public static ApiResponse<TValue> Success(TValue value, string message = "")
        => new(value, true, message);

    public static ApiResponse<TValue> Failure(TValue? value = null, string message = "")
        => new(value, false, message);
}
