using Application.Shared.ViewModels;
using Global.Abstractions.Areas.Account;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.Home.ViewModels;

public sealed class HomeVm : BasePageVm
{
    public HomeVm()
    {
        GiftingGroups = new List<IUserGiftingGroup>();
        GroupsRequiringSetup = new List<IUserGiftingGroup>();
    }

    public ISantaUser? CurrentUser { get; set; }
    public IList<IUserGiftingGroup> GiftingGroups { get; set; }
    public int NewJoinerApplicationsCount => GiftingGroups?.Sum(x => x.NewApplications) ?? 0;

    public int UnreadMessagesCount { get; set; }
    public int UnreadImportantMessagesCount { get; set; }
    
    public int GroupInvitationsCount { get; set; }
    public int GroupsNotInOrOutCount { get; set; }
    public int GroupsWithNoSuggestionsCount { get; set; }

    public IList<IUserGiftingGroup> GroupsRequiringSetup { get; set; }
    public int GroupsRequiringSetupCount => GroupsRequiringSetup.Count;

    public int PartnersAwaitingConfirmationCount { get; set; }

    public bool ShowHighlights => UnreadMessagesCount > 0 || NewJoinerApplicationsCount > 0 || GroupInvitationsCount > 0
         || GroupsNotInOrOutCount > 0 || GroupsWithNoSuggestionsCount > 0 || GroupsRequiringSetup.Any()
         || PartnersAwaitingConfirmationCount > 0;

    public string? InvitationWaitMessage { get; set; }
    public string? InvitationError { get; set; }
}
