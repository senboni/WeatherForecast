using System;

namespace WeatherForecast.Host.Extensions;

public static class HelperExtensions
{
    public static DateTime ToDateTime(this int unixTimestamp)
        => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp);
}
