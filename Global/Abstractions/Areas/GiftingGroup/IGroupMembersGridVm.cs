using Global.Abstractions.Shared;
using static Global.Settings.GiftingGroupSettings;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGroupMembersGridVm : IHaveAGroupKey
{
    OtherGroupMembersType MemberListType { get; }
    Guid? InvitationGuid { get; }
    IEnumerable<IGroupMember> OtherGroupMembers { get; set; }
}
