using Global.Abstractions.Shared;

namespace Global.Abstractions.Global;

public interface IHashableUser : IHashableUserBase, IHasGlobalUserId
{
    bool IdentificationHashed { get; set; }
}
