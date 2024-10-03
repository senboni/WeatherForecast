namespace WeatherForecast.Host.Common;

public static class Constants
{
    public const string OpenWeatherMapClient = "open-weather-map";
    public const string OpenWeatherMapApiKeyParameterName = "appid";
    public const string OpenWeatherMapBasePath = "/data/2.5";

    public static readonly string[] TemperatureUnits = ["C", "F", "K"];
}
