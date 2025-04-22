using Application.Shared.Identity;

namespace Application.Areas.Account.BaseModels;

public sealed class UnHashedUserWithGreeting : UserIdentificationBase
{
    public required string Greeting { get; set; }
}
