namespace ViewLayer.Models.Shared;

public class UserGridVm
{
    public UserGridVm(IQueryable<IVisibleUser> users, string userGridAction)
    {
        Users = users;
        UserGridAction = userGridAction;
    }

    public IQueryable<IVisibleUser> Users { get; set; }
    public string UserGridAction { get; }
}
