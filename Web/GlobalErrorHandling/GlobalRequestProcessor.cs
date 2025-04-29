using Global.Extensions.Exceptions;
using System.Runtime.ExceptionServices;

namespace Web.GlobalErrorHandling;

public sealed class GlobalRequestProcessor
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
        var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}";

        if (exception is NotSignedInException)
        {
            string requestUrl = context.Request.Path.ToString();
            context.Response.Redirect($"{baseUrl}/Account/Home/Login?ReturnUrl={requestUrl}&TimedOut=True");
        }
        else if (exception is AccessDeniedException)
        {
            context.Response.Redirect($"{baseUrl}/Account/Home/AccessDenied");
        }
        else if (exception is NotFoundException)
        {
            context.Response.Redirect($"{baseUrl}/Account/Home/NotFound");
        }
        else
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
