﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using WeatherForecast.Host.Common;
using WeatherForecast.Host.PipelineBehaviors;
using WeatherForecast.Host.WeatherProviders;

namespace WeatherForecast.Host.Extensions;

public static class ConfigureServices
{
    public static void AddMediatR(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(assembly);
    }

    public static void AddOpenWeatherMapWeatherProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenWeatherMapClient(configuration);
        services.AddSingleton<IWeatherProvider, OpenWeatherMapWeatherProvider>();
    }

    private static void AddOpenWeatherMapClient(this IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration["OpenWeatherMapBaseUrl"]
            ?? throw new Exception("'OpenWeatherMapBaseUrl' not set in configuration.");

        services.AddHttpClient<IWeatherProvider, OpenWeatherMapWeatherProvider>(Constants.OpenWeatherMapClient, client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("senboni/WeatherForecast");
        })
        .AddHttpMessageHandler(() => new OpenWeatherMapWeatherProvider.Handler(configuration));
    }
}
