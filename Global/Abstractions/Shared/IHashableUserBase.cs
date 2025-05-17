namespace Global.Abstractions.Shared;

public interface IHashableUserBase : IHasEmail
{
    string? UserName { get; set; }
}
