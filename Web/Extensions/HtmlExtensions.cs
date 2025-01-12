using Microsoft.AspNetCore.Html;

namespace Web.Extensions;

public static class HtmlExtensions
{
    /// <summary>
    /// Use this instead of Html.Raw() as it's safer
    /// </summary>
    public static HtmlString ToRawHtml(this string original)
    {
        return new HtmlString(original);
    }
}
