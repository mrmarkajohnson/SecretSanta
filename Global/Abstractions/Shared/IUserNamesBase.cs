using Global.Abstractions.Shared;

namespace Global.Abstractions.Global;

public interface IUserNamesBase : IUserAllNames, IHashableUser, IHasHashedUserId
{
}
