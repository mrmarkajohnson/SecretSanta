using Application.Areas.Home.ViewModels;
using Application.Areas.Messages.Queries;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.Home.Actions;

public sealed class AddHighlightsCountsAction : BaseAction<HomePageVm>
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

                var dbGroupLinks = dbCurrentSantaUser.GiftingGroupLinks
                    .Where(GroupUserExpressions.IsActive(true))
                    .Where(x => x.DateCreated.Year <= GlobalSettings.CurrentYear);

                var dbGroupYears = 

                _item.GroupsNotInOrOutCount = dbGroupLinks
                    .Count(x => x.GiftingGroup.Years
                        .Where(GroupYearExpressions.IsActive(false))
                        .Where(y => y.CalendarYear == GlobalSettings.CurrentYear)
                        .Where(y => y.Users
                            .Where(YearGroupUserExpressions.IsActive(false))
                            .Any(z => z.SantaUserKey == dbCurrentSantaUser.SantaUserKey && z.Included != null))
                        .Count() == 0);

                _item.GroupsWithNoSuggestionsCount = dbGroupLinks
                    .Count(x => x.GiftingGroup.Years
                        .Where(GroupYearExpressions.IsActive(false))
                        .Where(y => y.CalendarYear == GlobalSettings.CurrentYear)
                        .Where(y => y.Users.Any(z => z.SantaUserKey == dbCurrentSantaUser.SantaUserKey
                            && z.Suggestions.Any(SuggestionLinkExpressions.IsActive(false))))
                        .Count() == 0);

                var dbAdminGroupLinks = dbGroupLinks.Where(x => x.GroupAdmin);

                _item.GroupsRequiringSetup = dbAdminGroupLinks
                    .Where(x => x.GiftingGroup.Years
                        .Where(GroupYearExpressions.IsActive(false))
                        .Where(y => y.CalendarYear == GlobalSettings.CurrentYear)
                        .Where(y => y.Limit > 0 && y.Users.Any(z => z.RecipientSantaUserKey > 0))
                        .Count() == 0)
                    .AsQueryable()
                    .ProjectTo<IUserGiftingGroup>(Mapper.ConfigurationProvider)
                    .ToList();

                _item.PartnersAwaitingConfirmationCount = dbCurrentSantaUser.ConfirmingRelationships
                    .Where(PartnerLinkExpressions.IsActive(false))
                    .Where(PartnerLinkExpressions.SuggestingUserIsActive())
                    .Count(x => x.Confirmed == null);
            }
            catch { }
        }

        return true;
    }
}
