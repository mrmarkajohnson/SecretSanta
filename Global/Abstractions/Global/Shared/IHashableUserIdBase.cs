namespace Global.Abstractions.Global.Shared;

public interface IHashableUserIdBase
{
    string? UserName { get; set; }
    string? Email { get; set; }
}
