using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGroupMember : IUserNamesBase
{
    int SantaUserKey { get; }
    bool GroupAdmin { get; }
}
