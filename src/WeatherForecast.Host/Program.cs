using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.Features.CurrentWeather;
using WeatherForecast.Host.Features.ForecastWeather;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddOpenWeatherMapWeatherProvider(builder.Configuration);
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseGlobalExceptionHandler();

app.MapGet("/currentweather", GetCurrentWeather.Endpoint)
    .WithName("GetCurrentWeather")
    .WithOpenApi();

app.MapGet("/forecastweather", GetForecastWeather.Endpoint)
    .WithName("GetForecastWeather")
    .WithOpenApi();

app.Run();

public partial class Program { }