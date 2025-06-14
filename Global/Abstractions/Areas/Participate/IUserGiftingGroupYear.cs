using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.Shared;
using static Global.Settings.GiftingGroupSettings;

namespace Global.Abstractions.Areas.Participate;

public interface IUserGiftingGroupYear : IGiftingGroupYearBase, IUserGroupYearShared
{
    GroupMemberStatus MemberStatus { get; }
    IUserNamesBase? Recipient { get; }

    string LimitString { get; }
    string RecipientString { get; }
}
