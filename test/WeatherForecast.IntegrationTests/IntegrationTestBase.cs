namespace WeatherForecast.IntegrationTests;

public class IntegrationTestBase : IClassFixture<ApiFixture>
{
    protected readonly ApiFixture ApiFixture;

    public IntegrationTestBase(ApiFixture fixture)
    {
        ApiFixture = fixture;
    }
}
