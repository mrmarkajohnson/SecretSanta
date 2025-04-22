namespace Application.Areas.Account.BaseModels;

public class HashedUser
{
    public string? EmailHash { get; set; }
    public string UserNameHash { get; set; } = string.Empty;
}
