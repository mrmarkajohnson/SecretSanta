using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGroupMember : IUserAllNames
{
    int SantaUserKey { get; }
    bool GroupAdmin { get; }
}
