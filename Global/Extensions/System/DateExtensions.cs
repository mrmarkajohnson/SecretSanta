namespace Global.Extensions.System;

public static class DateExtensions
{
    public static string ToDateTimeString(this DateTime date)
    {
        return date.ToLongDateString() + " " + date.ToString("HH:mm");
    }
}
