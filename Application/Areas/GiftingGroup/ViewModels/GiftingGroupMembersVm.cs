using Global.Abstractions.Areas.GiftingGroup;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Areas.GiftingGroup.ViewModels;

public class GiftingGroupMembersVm : IGroupMembersGridVm
{
    public int GiftingGroupKey { get; init; }
    public OtherGroupMembersType MemberListType { get; init; }
    public Guid? InvitationGuid { get; set; }
    public required IEnumerable<IGroupMember> OtherGroupMembers { get; set; }
}
