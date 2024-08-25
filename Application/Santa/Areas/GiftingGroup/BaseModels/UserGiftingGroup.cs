using Global.Abstractions.Santa.Areas.GiftingGroup;

namespace Application.Santa.Areas.GiftingGroup.BaseModels;

public class UserGiftingGroup : IUserGiftingGroup
{
    public int GroupId { get; set; }
    public string GroupName { get; set; } = "";
    public bool GroupAdmin { get; set; }
}
