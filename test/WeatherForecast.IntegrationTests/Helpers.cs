using System.Text.Json;

namespace WeatherForecast.IntegrationTests;

public static class Helpers
{
    public static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
}
