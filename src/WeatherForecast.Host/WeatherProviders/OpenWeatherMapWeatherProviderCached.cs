using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherForecast.Host.WeatherProviders;

public class OpenWeatherMapWeatherProviderCached(
    OpenWeatherMapWeatherProvider weatherProvider,
    IMemoryCache memoryCache) : IWeatherProvider
{
    private readonly OpenWeatherMapWeatherProvider _weatherProvider = weatherProvider;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<Result<TValue, HttpStatusCode>> GetCurrentWeather<TValue>(string city, string unit, CancellationToken cancellationToken)
    {
        var key = $"current-{city}-{unit}";

        return await _memoryCache.GetOrCreateAsync(key, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            return await _weatherProvider.GetCurrentWeather<TValue>(city, unit, cancellationToken);
        });
    }

    public async Task<Result<TValue, HttpStatusCode>> GetForecastWeather<TValue>(string city, string unit, CancellationToken cancellationToken)
    {
        var key = $"forecast-{city}-{unit}";

        return await _memoryCache.GetOrCreateAsync(key, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            return await _weatherProvider.GetForecastWeather<TValue>(city, unit, cancellationToken);
        });
    }
}
