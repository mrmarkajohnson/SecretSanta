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

    public static string TrimEnd(this string text, string remove)
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(remove) || !text.Trim().EndsWith(remove))
        {
            return text;
        }
        else
        {
            return text[..^remove.Length];
        }

    }

    public static string DisplayList(this IEnumerable<string> list, bool or = false, bool oxfordComma = false)
    {
        if (list == null || list.Count() == 0)
        {
            return string.Empty;
        }
        else if (list.Count() == 1)
        {
            return list.ElementAt(0);
        }
        else
        {
            string result = "";
            string add = or ? "or" : "and";
            
            for (int i = 0; i < list.Count(); i++)
            {
                string element = list.ElementAt(i);

                if (i == 0)
                {
                    result = element;
                }
                else if (i < list.Count() - 1)
                {
                    result += ", " + element;
                }
                else
                {
                    result += (oxfordComma ? "," : "") +  $" {add} {element}";
                }
            }

            return result;
        }
    }
}
