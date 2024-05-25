namespace Application.Santa.Global;

/// <summary>
/// An Action is like a query but it transforms the object passed in, instead of retrieving somethig
/// It returns a simple success or failure boolean
/// </summary>
public abstract class BaseAction<TItem> : BaseRequest
{
    protected bool Success { get; set; }

    public abstract Task<bool> Handle();

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
