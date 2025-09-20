using Application.Shared.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.BaseModels;

public class GroupMember : UserNamesBase, IGroupMember
{
    public int SantaUserKey { get; set; }
    public bool GroupAdmin { get; set; }
}