namespace Global.Extensions.Exceptions;

public sealed class AccessDeniedException : Exception
{
    public AccessDeniedException()
    {
    }

    public AccessDeniedException(string? message) : base(message)
    {
    }
}
