using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WeatherForecast.Host.Common;

namespace WeatherForecast.Host.WeatherProviders;

public class OpenWeatherMapWeatherProvider(IHttpClientFactory httpClientFactory) : IWeatherProvider
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    private static readonly Dictionary<string, string> _units = new()
    {
        { "C", "metric" },
        { "F", "imperial" },
        { "K", "standard" },
    };

    public async Task<Result<TValue, HttpStatusCode>> GetCurrentWeather<TValue>(string city, string unit, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(Constants.OpenWeatherMapClient);
        var response = await client.GetAsync($"/weather?q={city}&units={_units[unit]}", cancellationToken);

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var value = await JsonSerializer.DeserializeAsync<TValue>(stream, cancellationToken: cancellationToken);

        if (value is null || !response.IsSuccessStatusCode)
        {
            return Result.Failure<TValue, HttpStatusCode>(response.StatusCode);
        }

        return Result.Success<TValue, HttpStatusCode>(value);
    }

    public async Task<Result<TValue, HttpStatusCode>> GetForecastWeather<TValue>(string city, string unit, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(Constants.OpenWeatherMapClient);
        var response = await client.GetAsync($"/forecast?q={city}&units={_units[unit]}", cancellationToken);

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var value = await JsonSerializer.DeserializeAsync<TValue>(stream, cancellationToken: cancellationToken);

        if (value is null || !response.IsSuccessStatusCode)
        {
            return Result.Failure<TValue, HttpStatusCode>(response.StatusCode);
        }

        return Result.Success<TValue, HttpStatusCode>(value);
    }

    public class Handler(IConfiguration configuration) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder(request.RequestUri!);
            uriBuilder.Path = $"{Constants.OpenWeatherMapBasePath}{uriBuilder.Path}";

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[Constants.OpenWeatherMapApiKeyParameterName] = configuration["OpenWeatherMapApiKey"];

            uriBuilder.Query = query.ToString();
            request.RequestUri = uriBuilder.Uri;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
