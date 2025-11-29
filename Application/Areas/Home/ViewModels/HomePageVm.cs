using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.Home.ViewModels;

public sealed class HomePageVm : HomeVm
{
    public HomePageVm()
    {
        GroupsRequiringSetup = new List<IUserGiftingGroup>();
    }

    public int UnreadMessagesCount { get; set; }
    public int UnreadImportantMessagesCount { get; set; }

    public int GroupsNotInOrOutCount { get; set; }
    public int GroupsWithNoSuggestionsCount { get; set; }

    public IList<IUserGiftingGroup> GroupsRequiringSetup { get; set; }
    public int GroupsRequiringSetupCount => GroupsRequiringSetup.Count;

    public int PartnersAwaitingConfirmationCount { get; set; }

    public bool ShowHighlights => UnreadMessagesCount > 0 || NewJoinerApplicationsCount > 0 || GroupInvitationsCount > 0
         || GroupsNotInOrOutCount > 0 || GroupsWithNoSuggestionsCount > 0 || GroupsRequiringSetup.Any()
         || PartnersAwaitingConfirmationCount > 0;
}
