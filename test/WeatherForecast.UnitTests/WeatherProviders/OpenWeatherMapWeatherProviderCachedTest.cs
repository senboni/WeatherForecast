using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using System.Net;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.UnitTests.WeatherProviders;

public class OpenWeatherMapWeatherProviderCachedTest
{
    [Fact]
    public async Task GetCurrentWeatherCalledTwice_ShouldCallHttpClientOnce()
    {
        //arrange
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        var httpClient = new HttpClient(new MockHandler())
        {
            BaseAddress = new Uri("https://mockurl.com"),
        };

        httpClientFactory.CreateClient(Constants.OpenWeatherMapClient).Returns(httpClient);

        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new OpenWeatherMapWeatherProvider(httpClientFactory);
        var cachedProvider = new OpenWeatherMapWeatherProviderCached(provider, cache);

        //act
        await cachedProvider.GetCurrentWeather<Result<object, HttpStatusCode>>("Barcelona", TemperatureUnit.C, default);
        await cachedProvider.GetCurrentWeather<Result<object, HttpStatusCode>>("Barcelona", TemperatureUnit.C, default);

        //assert
        httpClientFactory.Received(1).CreateClient(Constants.OpenWeatherMapClient);
    }

    [Fact]
    public async Task GetForecastWeatherCalled3Times_ShouldCallHttpClientTwice()
    {
        //arrange
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        var httpClient = new HttpClient(new MockHandler())
        {
            BaseAddress = new Uri("https://mockurl.com"),
        };

        httpClientFactory.CreateClient(Constants.OpenWeatherMapClient).Returns(httpClient);

        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new OpenWeatherMapWeatherProvider(httpClientFactory);
        var cachedProvider = new OpenWeatherMapWeatherProviderCached(provider, cache);

        //act
        await cachedProvider.GetForecastWeather<Result<object, HttpStatusCode>>("Barcelona", TemperatureUnit.F, default);
        await cachedProvider.GetForecastWeather<Result<object, HttpStatusCode>>("Barcelona", TemperatureUnit.F, default);
        await cachedProvider.GetForecastWeather<Result<object, HttpStatusCode>>("Barcelona", TemperatureUnit.C, default);

        //assert
        httpClientFactory.Received(2).CreateClient(Constants.OpenWeatherMapClient);
    }

    private class MockHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent("{\"someField\": \"someValue\"}"),
                StatusCode = HttpStatusCode.OK,
            });
    }
}
