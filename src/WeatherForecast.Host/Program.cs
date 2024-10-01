using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WeatherForecast.Host.Extensions;
using WeatherForecast.Host.Features.CurrentWeather;

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

app.MapGet("/currentweather", Endpoints.GetCurrentWeatherByCityEndpoint)
    .WithName("GetCurrentWeather")
    .WithOpenApi();

app.Run();