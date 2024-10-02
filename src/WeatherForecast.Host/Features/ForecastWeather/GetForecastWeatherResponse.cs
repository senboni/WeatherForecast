using System;
using System.Collections.Generic;

namespace WeatherForecast.Host.Features.ForecastWeather;

public class GetForecastWeatherResponse
{
    public required IEnumerable<Forecast> Forecasts { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }

    public class Forecast
    {
        public required string Description { get; init; }
        public required double Temperature { get; init; }
        public required double WindSpeed { get; init; }
        public required DateTime DateTime { get; init; }
    }
}
