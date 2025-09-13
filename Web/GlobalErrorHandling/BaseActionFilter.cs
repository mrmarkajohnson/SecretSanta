using Global.Settings;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.GlobalErrorHandling;

public class BaseActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var myController = context.Controller as BaseController;

        if (myController != null)
        {
            await myController.SetHomeModel();
            myController.ViewData["LayoutViewModel"] = myController.HomeModel;
            await MarkMessageRead(context, myController);
        }

        await next();
    }

    private static async Task MarkMessageRead(ActionExecutingContext context, BaseController myController)
    {
        try
        {
            int? messageKey = GetIntegerParameter(context, MessageSettings.FromMessageParameter);

            if (messageKey > 0)
            {
                int? recipientKey = GetIntegerParameter(context, MessageSettings.FromRecipientParameter);
                await myController.MarkMessageRead(messageKey.Value, recipientKey);
            }
        }
        catch { }
    }

    private static int? GetIntegerParameter(ActionExecutingContext context, string parameter)
    {
        try
        {
            if (context.HttpContext.Request.Query.ContainsKey(parameter))
            {
                if (int.TryParse(context.HttpContext.Request.Query[parameter].ElementAtOrDefault(0), out int result))
                    return result;
            }
        }
        catch { }

        try
        {
            if (context.ActionArguments.ContainsKey(parameter))
            {
                if (int.TryParse(context.ActionArguments[parameter]?.ToString(), out int result))
                    return result;
            }
        }
        catch { }

        return null;
    }
}