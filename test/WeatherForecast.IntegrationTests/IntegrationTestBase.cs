namespace WeatherForecast.IntegrationTests;

public class IntegrationTestBase(ApiFixture fixture) : IClassFixture<ApiFixture>
{
    protected readonly ApiFixture ApiFixture = fixture;
}
