# WeatherForecast API

This API provides weather data for a given city. It has two endpoints:
- **/currentweather**: Returns the current weather for a specified city.
- **/forecastweather**: Returns a weather forecast for a specified city, either for a specific date or a 5-day, 3-hour interval forecast.

## Endpoints

### 1. `/currentweather`

Retrieve the current weather for a specified city.

- **Method**: `GET`
- **Parameters**:
  - `city` (required, `string`): The name of the city for which to retrieve weather information.
  - `unit` (optional, `string`): Unit for temperature. Default is `c`. Options: `c,f,k`
    - `c`: Returns the temperature in Celsius.
    - `f`: Returns the temperature in Fahrenheit.
    - `k`: Returns the temperature in Kelvin.
    
- **Example Request**:

```bash
GET /currentweather?city=London&unit=fahrenheit
```
- **Example Response**:
```json
{
  "description": "Clouds, broken clouds, 10° C",
  "temperature": 10.13,
  "windSpeed": 2.57,
  "dateTime": "2024-10-03T22:30:42Z",
  "city": "Prague",
  "country": "CZ"
}
```

### 1. `/forecastweather`

Retrieve forecast weather for a specified city.

- **Method**: `GET`
- **Parameters**:
  - `city` (required, `string`): The name of the city for which to retrieve weather information.
  - `unit` (optional, `string`): Unit for temperature. Default is `c`. Options: `c,f,k`
    - `c`: Returns the temperature in Celsius.
    - `f`: Returns the temperature in Fahrenheit.
    - `k`: Returns the temperature in Kelvin.
  - `date` (optional, `string`): The date for which to retrieve the forecast. If not provided, a 5-day forecast in 3-hour intervals is returned. The date should be in format (YYYY-MM-DD HH:mm:ss).
    
- **Example Request with date**:

```bash
GET /forecastweather?city=Paris&unit=c&date=2024-10-06T18:00:00
```
- **Example Response**:
```json
{
  "forecasts": [
    {
      "description": "Rain, light rain, 15° C",
      "temperature": 15.2,
      "windSpeed": 4.08,
      "dateTime": "2024-10-06T21:00:00Z"
    }
  ],
  "city": "Paris",
  "country": "FR"
}
```
    
- **Example Request without date**:

```bash
GET /forecastweather?city=Paris&unit=c
```
- **Example Response**:
```json
{
  "forecasts": [
    {
      "description": "Clear, clear sky, 8° C",
      "temperature": 8.37,
      "windSpeed": 2.95,
      "dateTime": "2024-10-04T00:00:00Z"
    },
    //3 hour forecast in between...
    {
      "description": "Rain, light rain, 14° C",
      "temperature": 14.39,
      "windSpeed": 4.83,
      "dateTime": "2024-10-08T21:00:00Z"
    }
  ],
  "city": "Paris",
  "country": "FR"
}
```
## Error Handling

- 400 Bad Request: If required parameters are missing or invalid.
- 404 Not Found: If the city is not found by the weather provider.
- 500 Internal Server Error: If there is an issue with the weather provider or server.
- Example Error Responses:
```json
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "BadRequest",
    "status": 400,
    "detail": "City parameter must not be empty."
}
```
```json
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "BadRequest",
    "status": 400,
    "detail": "Invalid temperature unit. Available units: c (celsius), f (fahrenheit), k (kelvin)."
}
```
```json
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
    "title": "NotFound",
    "status": 404,
    "detail": "Unable to find city."
}
```

## Running the API Locally

To run this API locally, follow the steps below:

### Prerequisites

- **.NET 8**: Make sure you have .NET 8 installed on your machine.
  - You can download it from [here](https://dotnet.microsoft.com/download/dotnet/8.0).
- **Dotnet CLI**: Ensure that you have access to the `dotnet` CLI. You can verify this by running the following command in your terminal:

```bash
dotnet --version
```
- API Key: You will need an API key from [OpenWeatherMap](https://openweathermap.org/). This API key will be used to fetch weather data.

### Configuration
- Once you have obtained your OpenWeatherMap API key, you can:
  - add it directly to `appsettings.json` or
  - add it to the project using the dotnet CLI and the user-secrets feature.

User-secrets way: run the following command from project path to set up your API key:
```bash
dotnet user-secrets set "OpenWeatherMapApiKey" "<Your-API-Key>"
```

### Running the Project
1. Run the following commands from project path:
```bash
dotnet build
dotnet run
```
2. Access the Swagger UI:
```
http://localhost:<port>/swagger/index.html
```
Replace <port> with the port number where your API is running (typically shown in the console output when you run the project).

## Notes
- The API is based on real-time data from a weather provider.
- The forecast for a specified date will return the closest available data point to that date.