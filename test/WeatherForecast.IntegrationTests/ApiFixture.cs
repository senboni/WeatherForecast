using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Host.WeatherProviders;
using WeatherForecast.IntegrationTests.Mocks;

namespace WeatherForecast.IntegrationTests;

public class ApiFixture : WebApplicationFactory<Program>
{
    public HttpClient HttpClient { get; init; }

    public ApiFixture()
    {
        HttpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost"),
            AllowAutoRedirect = false,
        });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(AddMockWeatherProvider);
        builder.UseEnvironment("Development");
    }

    private static void AddMockWeatherProvider(IServiceCollection services)
    {
        ServiceDescriptor? weatherProvider;

        while ((weatherProvider = services.FirstOrDefault(x => x.ServiceType == typeof(IWeatherProvider))) is not null)
        {
            services.Remove(weatherProvider);
        }

        services.AddSingleton<IWeatherProvider, MockWeatherProvider>();
    }
}
