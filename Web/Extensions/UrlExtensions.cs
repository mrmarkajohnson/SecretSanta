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
        object fullValues = new { Area = area };

        if (values != null)
        {
            var dictionary = values.ToDictionary();

            foreach (var value in dictionary)
            {
                fullValues = fullValues.AddProperty(value.Key, value.Value);
            }
        }

        return urlHelper.Action(action, controller, fullValues, request.Scheme, request.Host.ToString()) ?? "";
    }
}
