using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.BaseModels;

public class UserGiftingGroupYear : IUserGiftingGroupYear
{
    public int GiftingGroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public bool GroupAdmin { get; set; }
    public bool Included { get; set; }
    public IUserNamesBase? Recipient { get; set; }
}
