using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.BaseModels;

public sealed class UserGiftingGroup : IUserGiftingGroup
{
    public int GiftingGroupKey { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public bool GroupAdmin { get; set; }
    public int NewApplications { get; set; }
    public int CurrentYear { get; set; }
}
