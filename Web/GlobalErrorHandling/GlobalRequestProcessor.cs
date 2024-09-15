using Global.Extensions.Exceptions;
using System.Runtime.ExceptionServices;

namespace Web.GlobalErrorHandling;

public class GlobalRequestProcessor
{
    private readonly RequestDelegate _next;

    public GlobalRequestProcessor(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            HandleException(context, ex);
        }
    }

    private static void HandleException(HttpContext context, Exception exception)
    {
        if (exception is NotSignedInException)
        {
            string requestUrl = context.Request.Path.ToString();
            context.Response.Redirect($"Account/Home/Login?ReturnUrl={requestUrl}&TimedOut=True");
        }
        else if (exception is AccessDeniedException)
        {
            context.Response.Redirect($"Account/Home/AccessDenied");
        }
        else if (exception is NotFoundException)
        {
            context.Response.Redirect($"Account/Home/NotFound");
        }
        else
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
