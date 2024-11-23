namespace Application.Santa.Areas.Account.BaseModels;

public class HashedUserIdWithGreeting : HashedUserId
{
    public string GreetingHash { get; set; } = string.Empty;
}
