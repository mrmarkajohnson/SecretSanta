namespace Global.Helpers;

public static class DateHelper
{
    public static DateTime FirstDayOfNextYear(int calendarYear)
    {
        return new DateTime(calendarYear + 1, 1, 1);
    }
}