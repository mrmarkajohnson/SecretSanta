namespace Global.Abstractions.Shared;

public interface IHashableUserBase : IHaveAnEmail
{
    string? UserName { get; set; }
}
