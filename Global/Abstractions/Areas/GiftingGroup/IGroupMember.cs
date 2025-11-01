using Global.Abstractions.Shared;
using static Global.Settings.GiftingGroupSettings;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGroupMember : IUserNamesBase
{
    int SantaUserKey { get; }
    GroupMemberStatus MemberStatus { get; }
    int? GroupApplicationKey { get; }
}
