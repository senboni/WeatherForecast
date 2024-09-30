using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Application.WeatherProviders;

namespace WeatherForecast.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IWeatherProvider, OpenWeatherMapWeatherProvider>();
        services.AddOpenWeatherMapClient(configuration);
    }

    private static void AddOpenWeatherMapClient(this IServiceCollection services, IConfiguration configuration)
    {
        var clientName = configuration["OpenWeatherMapClientName"]
            ?? throw new Exception("'OpenWeatherMapClient' not set in configuration.");

        var baseUrl = configuration["OpenWeatherMapBaseUrl"]
            ?? throw new Exception("'OpenWeatherMapBaseUrl' not set in configuration.");

        services.AddHttpClient<IWeatherProvider, OpenWeatherMapWeatherProvider>(clientName, client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("senboni/WeatherForecast");
        });
    }
}
