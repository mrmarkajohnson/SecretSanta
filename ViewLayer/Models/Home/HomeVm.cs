using Global.Abstractions.Santa.Areas.Account;

namespace ViewLayer.Models.Home;

public class HomeVm : BasePageVm
{
    public ISantaUser? CurrentUser { get; set; }
}
