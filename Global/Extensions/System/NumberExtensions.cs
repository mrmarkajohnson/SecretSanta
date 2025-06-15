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
}
