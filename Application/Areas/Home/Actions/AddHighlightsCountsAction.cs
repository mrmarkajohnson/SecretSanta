using Application.Areas.Home.ViewModels;
using Application.Areas.Messages.Queries;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.Home.Actions;

public class AddHighlightsCountsAction : BaseAction<HomePageVm>
{
    public AddHighlightsCountsAction(HomePageVm item)
    {
        _item = item;
    }

    private readonly HomePageVm _item;

    protected async override Task<bool> Handle()
    {
        var messages = await Send(new GetMessagesQuery());
        var unreadMessages = messages.Where(x => !x.Read).ToList();
        _item.UnreadMessagesCount = unreadMessages.Count;
        _item.UnreadImportantMessagesCount = unreadMessages.Where(x => x.Important).Count();

        if (DateTime.Today.Month >= 11)
        {
            try
            {
                Santa_User dbCurrentSantaUser = GetCurrentSantaUser();

                var dbGroupLinks = dbCurrentSantaUser.GiftingGroupLinks.Where(x => x.DateDeleted == null && x.DateArchived == null);

                _item.GroupsNotInOrOutCount = dbGroupLinks
                    .Where(x => x.GiftingGroup.Years
                        .Where(y => y.CalendarYear >= GlobalSettings.CurrentYear && y.DateDeleted == null)
                        .Where(y => y.Users.Any(z => z.SantaUserKey == dbCurrentSantaUser.SantaUserKey && z.Included != null))
                        .Count() == 0)
                    .Count();

                _item.GroupsWithNoSuggestionsCount = dbGroupLinks
                    .Where(x => x.GiftingGroup.Years
                        .Where(y => y.CalendarYear >= GlobalSettings.CurrentYear && y.DateDeleted == null)
                        .Where(y => y.Users.Any(z => z.SantaUserKey == dbCurrentSantaUser.SantaUserKey && z.Suggestions.Any()))
                        .Count() == 0)
                    .Count();

                var dbAdminGroupLinks = dbGroupLinks.Where(x => x.GroupAdmin);

                _item.GroupsRequiringSetup = dbAdminGroupLinks
                    .Where(x => x.GiftingGroup.Years
                        .Where(y => y.CalendarYear >= GlobalSettings.CurrentYear && y.DateDeleted == null)
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
            catch { }
        }

        return true;
    }
}
