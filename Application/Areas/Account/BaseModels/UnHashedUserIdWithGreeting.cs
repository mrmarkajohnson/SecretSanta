namespace Application.Areas.Account.BaseModels;

public class UnHashedUserIdWithGreeting : UnHashedUserId
{
    public required string Greeting { get; set; }
}
