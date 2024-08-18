using Global.Extensions.System;
using Xunit;

namespace UnitTests.ExtensionTests;

public class TestTimeSpanExtensions
{
    [Fact]
    public void TestTimeSpanDescription()
    {
        var fiveHoursOnly = new TimeSpan(5, 0, 0);
        string fiveHoursOnlyDescription = fiveHoursOnly.GetDescription();
        Assert.Equal("5 hours", fiveHoursOnlyDescription);

        var fiveHoursTwenty = new TimeSpan(5, 20, 0);
        string fiveHoursTwentyDescription = fiveHoursTwenty.GetDescription();
        Assert.Equal("5 hours and 20 minutes", fiveHoursTwentyDescription);

        var fiveHoursTwentyAndAHalf = new TimeSpan(5, 20, 30);
        string fiveHoursTwentyAndAHalfDescription = fiveHoursTwentyAndAHalf.GetDescription();
        Assert.Equal("5 hours, 20 minutes and 30 seconds", fiveHoursTwentyAndAHalfDescription);

        var twoDaysfiveHoursTwentyAndAHalf = new TimeSpan(2, 5, 20, 30);
        string twoDaysfiveHoursTwentyAndAHalfDescription = twoDaysfiveHoursTwentyAndAHalf.GetDescription();
        Assert.Equal("2 days, 5 hours, 20 minutes and 30 seconds", twoDaysfiveHoursTwentyAndAHalfDescription);

        var threeHundredAndSixtyMinutes = TimeSpan.FromMinutes(360);
        string threeHundredAndSixtyMinutesDescription = threeHundredAndSixtyMinutes.GetDescription();
        Assert.Equal("6 hours", threeHundredAndSixtyMinutesDescription);
    }
}
