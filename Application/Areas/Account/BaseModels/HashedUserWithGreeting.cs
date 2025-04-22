namespace Application.Areas.Account.BaseModels;

public sealed class HashedUserWithGreeting : HashedUser
{
    public string GreetingHash { get; set; } = string.Empty;
}
