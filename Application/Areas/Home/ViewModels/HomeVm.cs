using Application.Shared.ViewModels;
using Global.Abstractions.Areas.Account;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.Home.ViewModels;

public class HomeVm : BasePageVm
{
    public HomeVm()
    {
        GiftingGroups = new List<IUserGiftingGroup>();
    }

    public ISantaUser? CurrentUser { get; set; }
    public IList<IUserGiftingGroup> GiftingGroups { get; set; }

    public int NewJoinerApplicationsCount => GiftingGroups?.Sum(x => x.NewApplications) ?? 0;
    public int GroupInvitationsCount { get; set; }
    
    public string? InvitationWaitMessage { get; set; }
    public string? InvitationError { get; set; }

    public bool EmailConfirmationNeeded => CurrentUser?.EmailConfirmed == false && CurrentUser.Email.IsNotEmpty();
}
