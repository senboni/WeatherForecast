using WeatherForecast.Host.Common;

namespace WeatherForecast.UnitTests.Common;

public class MakeTests
{
    [Fact]
    public void WeatherDescription_GivenValidParameters_ShouldFormatTemperatureAndUnit_AndHavePartsAtFront()
    {
        //arrange
        var temp = 22.5;
        var unit = "C";
        string[] parts = ["Nice", "Very nice"];
        var expected = "Nice, Very nice, 22° C";

        //act
        var actual = Make.WeatherDescription(temp, unit, parts);

        //assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void WeatherDescription_GivenSomeEmptyParts_ShouldNotIncludeEmptyParts()
    {
        //arrange
        var temp = 15;
        var unit = "C";
        string?[] parts = ["", null, "Cloudy", ""];
        var expected = "Cloudy, 15° C";

        //act
        var actual = Make.WeatherDescription(temp, unit, parts);

        //assert
        Assert.Equal(expected, actual);
    }
}
