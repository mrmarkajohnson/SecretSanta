using Application.Areas.Home.ViewModels;
using Application.Areas.Messages.Queries;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.Home.Actions;

public class AddHighlightsCountsAction : BaseAction<HomeVm>
{
    public AddHighlightsCountsAction(HomeVm item)
    {
        _item = item;
    }

    private readonly HomeVm _item;

    protected async override Task<bool> Handle()
    {
        var messages = await Send(new GetMessagesQuery());
        var unreadMessages = messages.Where(x => !x.Read).ToList();
        _item.UnreadMessagesCount = unreadMessages.Count;
        _item.UnreadImportantMessagesCount = unreadMessages.Where(x => x.Important).Count();

        try
        {
            Santa_User dbCurrentSantaUser = GetCurrentSantaUser();

            _item.GroupInvitationsCount = dbCurrentSantaUser.ReceivedInvitations
                .Where(x => x.DateArchived == null)
                .Count();

            if (DateTime.Today.Month >= 11)
            {
                var dbGroupLinks = dbCurrentSantaUser.GiftingGroupLinks.Where(x => x.DateDeleted == null && x.DateArchived == null);
                int currentYear = DateTime.Today.Year;

                _item.GroupsNotInOrOutCount = dbGroupLinks
                    .Where(x => x.GiftingGroup.Years
                        .Where(y => y.CalendarYear == currentYear && y.DateDeleted == null)
                        .Where(y => y.Users.Any(z => z.SantaUserKey == dbCurrentSantaUser.SantaUserKey && z.Included != null))
                        .Count() == 0)
                    .Count();

                _item.GroupsWithNoSuggestionsCount = dbGroupLinks
                    .Where(x => x.GiftingGroup.Years
                        .Where(y => y.CalendarYear == currentYear && y.DateDeleted == null)
                        .Where(y => y.Users.Any(z => z.SantaUserKey == dbCurrentSantaUser.SantaUserKey && z.Suggestions.Any()))
                        .Count() == 0)
                    .Count();

                var dbAdminGroupLinks = dbGroupLinks.Where(x => x.GroupAdmin);

                _item.GroupsRequiringSetup = dbAdminGroupLinks
                    .Where(x => x.GiftingGroup.Years
                        .Where(y => y.CalendarYear == currentYear && y.DateDeleted == null)
                        .Where(y => y.Limit > 0 && y.Users.Any(z => z.RecipientSantaUserKey > 0))
                        .Count() == 0)
                    .AsQueryable()
                    .ProjectTo<IUserGiftingGroup>(Mapper.ConfigurationProvider)
                    .ToList();

                _item.PartnersAwaitingConfirmationCount = dbCurrentSantaUser.ConfirmingRelationships
                    .Where(x => x.DateDeleted == null && x.DateArchived == null)
                    .Where(x => x.Confirmed == null)
                    .Count();
            }
        }
        catch { }

        return true;
    }
}
