using System.Security.Claims;

namespace Application.Santa.Global;

/// <summary>
/// An Action is like a query but it transforms the object passed in, instead of retrieving somethig
/// It returns a simple success or failure boolean
/// </summary>
public abstract class BaseAction<TItem> : BaseRequest<bool>
{
    protected bool Success { get; set; }

    public override async Task<bool> Handle(IServiceProvider services, ClaimsPrincipal claimsUser)
    {
        Initialise(services, claimsUser);
        return await Handle();
    }

    protected abstract Task<bool> Handle();

    public bool SuccessResult
    { 
        get 
        {
            Success = true;
            return Success;
        } 
    }

    public bool FailureResult
    {
        get
        {
            Success = false;
            return Success;
        }
    }

    public bool Result => Success;
}
