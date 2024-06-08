using System.Text.RegularExpressions;

namespace Global.Abstractions.Extensions;

public static class StringExtensions
{
    public static string SplitPascalCase(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        
        Regex r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");

        return r.Replace(value, " ");
    }
}
