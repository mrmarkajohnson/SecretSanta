using Application.Shared.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Areas.GiftingGroup.BaseModels;

public class GroupMember : UserNamesBase, IGroupMember
{
    public int SantaUserKey { get; set; }

    [Display(Name = GiftingGroupNames.MemberStatus)]
    public GroupMemberStatus MemberStatus { get; set; }
}