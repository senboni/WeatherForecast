using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WeatherForecast.Host.WeatherProviders;
using WeatherForecast.IntegrationTests.Mocks;

namespace WeatherForecast.IntegrationTests;

public class BrokenApiFixture : ApiFixture
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IWeatherProvider));
            services.AddSingleton<IWeatherProvider, BrokenWeatherProviderMock>();
        });

        builder.UseEnvironment("Development");
    }
}
