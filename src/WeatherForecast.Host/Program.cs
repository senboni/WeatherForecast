using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.Features.CurrentWeather;
using WeatherForecast.Host.Features.ForecastWeather;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR();
builder.Services.AddOpenWeatherMapWeatherProvider(builder.Configuration);
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog(
    (context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/currentweather", GetCurrentWeatherRequest.Endpoint)
    .WithName("GetCurrentWeather")
    .WithOpenApi();

app.MapGet("/forecastweather", GetForecastWeatherRequest.Endpoint)
    .WithName("GetForecastWeather")
    .WithOpenApi();

app.Run();