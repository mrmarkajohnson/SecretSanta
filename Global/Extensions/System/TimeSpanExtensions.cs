namespace Global.Extensions.System;

public static class TimeSpanExtensions
{
    public static string GetDescription(this TimeSpan timeSpan)
    {
        var list = new List<string>();

        AddToTimeSpanDescription(list, timeSpan.Days, "days");
        AddToTimeSpanDescription(list, timeSpan.Hours, "hours");
        AddToTimeSpanDescription(list, timeSpan.Minutes, "minutes");
        AddToTimeSpanDescription(list, timeSpan.Seconds, "seconds");
        AddToTimeSpanDescription(list, timeSpan.Milliseconds, "milliseconds");

        return list.DisplayList();
    }

    private static void AddToTimeSpanDescription(List<string> list, int number, string unit)
    {
        if (number > 0)
        {
            list.Add(number.ToString() + " " + unit);
        }
    }
}
