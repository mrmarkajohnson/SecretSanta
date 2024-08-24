namespace Global.Extensions.Exceptions;

public class NotFoundException : Exception
{
	public NotFoundException(string itemType)
	{
		Message = $"{itemType} not found";
	}

	public override string Message { get; }
}
