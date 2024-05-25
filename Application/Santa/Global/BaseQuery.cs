namespace Application.Santa.Global;

public abstract class BaseQuery<TItem> : BaseRequest
{
    public abstract Task<TItem> Handle();
}
