using System.Security.Claims;

namespace Application.Santa.Global;

public abstract class BaseQuery<TItem> : BaseRequest<TItem>
{
    public override Task<TItem> Handle(IServiceProvider services, ClaimsPrincipal claimsUser)
    {
        Initialise(services, claimsUser);
        return Handle();
    }

    protected abstract Task<TItem> Handle();
}
