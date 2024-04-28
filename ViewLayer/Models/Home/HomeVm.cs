using Global.Abstractions.Santa.Areas.Account;

namespace ViewLayer.Models.Home;

public class HomeVm
{
    public ISantaUser? CurrentUser { get; set; }
}
