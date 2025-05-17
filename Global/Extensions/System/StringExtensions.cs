using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Global.Extensions.System;

public static class StringExtensions
{
    public static string SplitPascalCase(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;

        //Regex r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");

        //return r.Replace(value, " ");

        return Regex.Replace(Regex.Replace(value, "([a-z])([A-Z])", "$1 $2"), "([A-Z])([A-Z])([a-z])", "$1 $2$3");
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
            string result = string.Empty;
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
                    result += (oxfordComma ? "," : string.Empty) + $" {add} {element}";
                }
            }

            return result;
        }
    }

    public static string Tidy(this string text)
    {
        return text.Trim().Replace("  ", " ").Replace("  ", " ");
    }

    public static string? Tidy(this string? text, bool emptyStringIfNull)
    {
        if (text == null)
            return emptyStringIfNull ? "" : null;

        return Tidy(text);
    }

    public static string? NullIfEmpty(this string? text)
    {
        return string.IsNullOrWhiteSpace(text) ? null : text.Trim();
    }

    public static bool NotEmpty([NotNullWhen(true)]this string? text)
    {
        return !string.IsNullOrWhiteSpace(text);
    }

    public static bool ContainsWord(this string? text, string? word)
    {
        if (text.NotEmpty() && word.NotEmpty())
        {
            text = text.Trim();
            word = word.Trim();
            
            return text == word || text.Contains(word + " ") || text.Contains(" " + word);
        }

        return false;
    }
}
