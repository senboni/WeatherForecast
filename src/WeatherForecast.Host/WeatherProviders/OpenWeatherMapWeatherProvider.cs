using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WeatherForecast.Host.Common;

namespace WeatherForecast.Host.WeatherProviders;

public class OpenWeatherMapWeatherProvider(IHttpClientFactory httpClientFactory) : IWeatherProvider
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<HttpResponseMessage> GetCurrentWeather(string city)
    {
        var client = _httpClientFactory.CreateClient(Constants.OpenWeatherMapClient);
        return await client.GetAsync($"/weather?q={city}");
    }

    public async Task<HttpResponseMessage> GetForecastWeather(string city)
    {
        var client = _httpClientFactory.CreateClient(Constants.OpenWeatherMapClient);
        return await client.GetAsync($"/forecast?q={city}");
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
