using Global.Abstractions.Shared;

namespace Global.Abstractions.Global;

public interface IHashableUserBase : IHasEmail
{
    string? UserName { get; set; }
}
