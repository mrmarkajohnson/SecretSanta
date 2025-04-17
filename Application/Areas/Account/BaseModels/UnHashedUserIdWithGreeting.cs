namespace Application.Areas.Account.BaseModels;

public sealed class UnHashedUserIdWithGreeting : UnHashedUserId
{
    public required string Greeting { get; set; }
}
