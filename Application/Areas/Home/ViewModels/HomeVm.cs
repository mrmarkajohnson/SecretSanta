using Application.Shared.ViewModels;
using Global.Abstractions.Areas.Account;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.Home.ViewModels;

public sealed class HomeVm : BasePageVm
{
    public HomeVm()
    {
        GiftingGroups = new List<IUserGiftingGroup>();
    }

    public ISantaUser? CurrentUser { get; set; }
    public IList<IUserGiftingGroup> GiftingGroups { get; set; }
    public int NewJoinerApplications => GiftingGroups?.Sum(x => x.NewApplications) ?? 0;
    public string? InvitationError { get; set; }
}
