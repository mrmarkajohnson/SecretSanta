namespace Global.Helpers;

public static class UrlHelper
{
    public static string ParameterDelimiter(string url)
    {
        return (url.Contains("?") ? "&" : "?");
    }
}
