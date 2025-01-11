using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.BaseModels;

public class UserGiftingGroup : IUserGiftingGroup
{
    public int GiftingGroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public bool GroupAdmin { get; set; }
    public int NewApplications { get; set; }
}
