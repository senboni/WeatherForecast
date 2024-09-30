using WeatherForecast.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", () => new object[] { new { TemperatureC = 25, TemperatureF = 77 } })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();
