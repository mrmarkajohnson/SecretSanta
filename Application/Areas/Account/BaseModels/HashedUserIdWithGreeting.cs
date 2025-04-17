namespace Application.Areas.Account.BaseModels;

public sealed class HashedUserIdWithGreeting : HashedUserId
{
    public string GreetingHash { get; set; } = string.Empty;
}
