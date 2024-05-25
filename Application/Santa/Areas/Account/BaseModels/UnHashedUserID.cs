namespace Application.Santa.Areas.Account.BaseModels;

public class UnHashedUserId
{
    public string? Email { get; set; }
    public required string UserName { get; set; }
}