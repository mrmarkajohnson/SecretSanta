using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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

    public static ViewDataDictionary TooltipContainerVd(this ViewDataDictionary dictionary, string element)
    {
        return new ViewDataDictionary(dictionary)
        {
            { "data-bs-container", element }
        };
    }
}
