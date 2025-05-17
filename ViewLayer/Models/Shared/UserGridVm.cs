using Global.Abstractions.Shared;

namespace ViewLayer.Models.Shared;

public sealed class UserGridVm
{
    public UserGridVm(IQueryable<IVisibleUser> users, string userGridAction)
    {
        Users = users;
        UserGridAction = userGridAction;
    }

    public IQueryable<IVisibleUser> Users { get; set; }
    public string UserGridAction { get; }
}
