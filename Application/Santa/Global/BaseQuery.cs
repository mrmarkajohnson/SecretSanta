namespace Application.Santa.Global;

public abstract class BaseQuery<TItem> : BaseRequest<TItem>
{
    public override Task<TItem> Handle(IServiceProvider services)
    {
        Initialise(services);
        return Handle();
    }

    protected abstract Task<TItem> Handle();
}
