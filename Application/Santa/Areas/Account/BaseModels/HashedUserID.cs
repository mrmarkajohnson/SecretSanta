namespace Application.Santa.Areas.Account.BaseModels;

public class HashedUserId
{
    public string? EmailHash { get; set; }
    public required string UserNameHash { get; set; }
}
