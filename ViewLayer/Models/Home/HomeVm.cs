using Global.Abstractions.Santa.Areas.Account;
using Global.Abstractions.Santa.Areas.GiftingGroup;

namespace ViewLayer.Models.Home;

public class HomeVm : BasePageVm
{
    public HomeVm()
    {
        GiftingGroups = new List<IUserGiftingGroup>();
    }
    
    public ISantaUser? CurrentUser { get; set; }
    public IList<IUserGiftingGroup> GiftingGroups { get; set; }
}
