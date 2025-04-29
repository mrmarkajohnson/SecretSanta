using System.Security.Claims;

namespace Application.Shared.Requests;

public abstract class BaseQuery<TItem> : BaseRequest<TItem>
{
    public override Task<TItem> Handle(IServiceProvider services, ClaimsPrincipal claimsUser)
    {
        Initialise(services, claimsUser);
        return Handle();
    }

    protected abstract Task<TItem> Handle();

    protected Task<TItem> Result(TItem item)
    {
        return Task.FromResult(item);
    }
}
