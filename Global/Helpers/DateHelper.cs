namespace Global.Helpers;

public static class DateHelper
{
    public static DateTime FirstDayOfNextYear(int year)
    {
        return new DateTime(year + 1, 1, 1);
    }
}
