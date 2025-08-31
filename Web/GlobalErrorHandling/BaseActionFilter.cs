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
        var actionArguments = context.ActionArguments;

        if (actionArguments.ContainsKey(MessageSettings.FromMessageParameter)
            && actionArguments.ContainsKey(MessageSettings.FromRecipientParameter))
        {
            try
            {
                int? messageKey = (int?)actionArguments[MessageSettings.FromMessageParameter];
                int? recipientKey = (int?)actionArguments[MessageSettings.FromRecipientParameter];

                if (messageKey > 0 && recipientKey > 0)
                {
                    await myController.MarkMessageRead(messageKey.Value, recipientKey);
                }
            }
            catch
            {
                // just continue
            }
        }
    }
}