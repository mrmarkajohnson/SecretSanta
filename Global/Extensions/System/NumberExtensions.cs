namespace Global.Extensions.System;

public static class NumberExtensions
{
    public static bool IsEmpty(this int? value)
    {
        return value == null || value == 0;
    }

    public static bool IsEmpty(this decimal? value)
    {
        return value == null || value == 0;
    }

    public static string Ordinal(this int value)
    {
        return value switch
        {
            11 => "th",
            12 => "th",
            13 => "th",
            _ => (Math.Abs(value) % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            }
        };
    }
}
