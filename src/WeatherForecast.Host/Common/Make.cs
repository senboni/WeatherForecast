using System.Linq;

namespace WeatherForecast.Host.Common;

public static class Make
{
    public static string WeatherDescription(double temp, string unit, params string?[] parts)
    {
        string?[] allPartsArranged = [.. parts, $"{(int)temp}° {unit}"];
        return string.Join(", ", allPartsArranged.Where(x => !string.IsNullOrEmpty(x)));
    }
}
