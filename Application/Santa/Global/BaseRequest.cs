using SecretSanta.Data;

namespace Application.Santa.Global;

public abstract class BaseRequest
{
    protected ApplicationDbContext ModelContext { get; set; }

    protected BaseRequest()
    {
        ModelContext = new ApplicationDbContext();
    }
}
