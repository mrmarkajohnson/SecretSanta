namespace Application.Areas.Account.BaseModels;

public class UnHashedUserBase : IHashableUserBase
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
}