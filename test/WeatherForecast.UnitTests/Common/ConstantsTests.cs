using WeatherForecast.Host.Common;

namespace WeatherForecast.UnitTests.Common;

public class ConstantsTests
{
    [Fact]
    public void TemperatureUnits_MustRemainUntouched()
    {
        Assert.Equal(["C", "F", "K"], Constants.TemperatureUnits);
    }
}
