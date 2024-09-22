using Microsoft.AspNetCore.Mvc.Filters;
using Web.Controllers;

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
        }

        await next();
    }
}