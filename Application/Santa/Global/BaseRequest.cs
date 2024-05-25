using SecretSanta.Data;

namespace Application.Santa.Global;

public abstract class BaseRequest
{
    protected ApplicationDbContext ModelContext { get; set; }

    protected BaseRequest()
    {
        ModelContext = new ApplicationDbContext();
    }

    protected async Task<TItem> Send<TItem>(BaseQuery<TItem> query)
    {
        return await query.Handle();
    }

    protected async Task<bool> Send<TItem>(BaseAction<TItem> action)
    {
        return await action.Handle();
    }
}
