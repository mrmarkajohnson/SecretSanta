using Global.Abstractions.Global.Shared;

namespace Application.Santa.Areas.Account.BaseModels;

public class UnHashedUserId : IHashableUserIdBase
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
}