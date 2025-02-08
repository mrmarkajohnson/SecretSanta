namespace Global.Extensions.Exceptions;

public class NotFoundException : Exception
{
	public NotFoundException(string itemDescription)
	{
		Message = $"{itemDescription} was not found";
	}

	public override string Message { get; }
}
