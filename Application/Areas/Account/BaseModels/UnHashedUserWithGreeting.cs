namespace Application.Areas.Account.BaseModels;

public sealed class UnHashedUserWithGreeting : UnHashedUserBase
{
    public required string Greeting { get; set; }
}
