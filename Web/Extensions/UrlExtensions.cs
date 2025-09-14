using System;

namespace Web.Helpers;

public static class UrlExtensions
{
    public static string Action(this IUrlHelper urlHelper, HttpContext context,
        string action, string controller, string area, object? values = null)
    {
        return urlHelper.Action(context.Request, action, controller, area, values);
    }

    public static string Action(this IUrlHelper urlHelper, HttpRequest request,
        string action, string controller, string area, object? values = null)
    {
        controller = controller.TrimEnd("Controller");
        object fullValues = GetUrlValues(area, values);
        return urlHelper.Action(action, controller, fullValues, request.Scheme, request.Host.ToString()) ?? "";
    }

    public static string Action(this IUrlHelper urlHelper, string action, string controller, string area, object? values = null)
    {
        controller = controller.TrimEnd("Controller");
        object fullValues = GetUrlValues(area, values);
        return urlHelper.Action(action, controller, fullValues) ?? "";
    }

    private static object GetUrlValues(string area, object? values)
    {
        object fullValues = new { Area = area };

        if (values != null)
        {
            var dictionary = values.ToDictionary();

            foreach (var value in dictionary)
            {
                fullValues = fullValues.AddProperty(value.Key, value.Value);
            }
        }

        return fullValues;
    }
}
