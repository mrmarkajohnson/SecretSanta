namespace Application.Shared.Helpers;

public static class TextHelper
{
    public static string TrimEnd(this string text, string remove)
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(remove)|| !text.Trim().EndsWith(remove))
        {
            return text;
        }
        else
        {
            return text[..^remove.Length];
        }

    }
}
