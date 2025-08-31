namespace Global.Extensions.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string itemDescription)
    {
        Message = $"The requested {itemDescription} could not be found; you may not have access to it. Please go back and try again.";
    }

    public override string Message { get; }
}
