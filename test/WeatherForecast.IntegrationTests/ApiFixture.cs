using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WeatherForecast.Host.WeatherProviders;
using WeatherForecast.IntegrationTests.Mocks;

namespace WeatherForecast.IntegrationTests;

public class ApiFixture : WebApplicationFactory<Program>
{
    public HttpClient HttpClient { get; init; }

    public ApiFixture()
    {
        HttpClient = CreateClient();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IWeatherProvider));
            services.AddSingleton<IWeatherProvider, MockWeatherProvider>();
        });

        builder.UseEnvironment("Development");
    }
}
