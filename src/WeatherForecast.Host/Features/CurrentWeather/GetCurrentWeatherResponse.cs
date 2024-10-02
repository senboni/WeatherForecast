using System;

namespace WeatherForecast.Host.Features.CurrentWeather;

public class GetCurrentWeatherResponse
{
    public required string Description { get; init; }
    public required double Temperature { get; init; }
    public required double WindSpeed { get; init; }
    public required DateTime DateTime { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
}
