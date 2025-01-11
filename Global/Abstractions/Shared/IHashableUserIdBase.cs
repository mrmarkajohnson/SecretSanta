namespace Global.Abstractions.Global;

public interface IHashableUserIdBase
{
    string? UserName { get; set; }
    string? Email { get; set; }
}
