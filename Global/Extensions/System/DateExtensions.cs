using System.Globalization;

namespace Global.Extensions.System;

public static class DateExtensions
{
    public static string ToDateTimeString(this DateTime date)
    {
        return date.Dayth() + " " + date.ToString("MMM yyyy HH:mm", CultureInfo.InvariantCulture);
    }

    public static string Dayth(this DateTime date)
    {
        return date.Day.ToString() + date.Day.Ordinal();
    }
}
