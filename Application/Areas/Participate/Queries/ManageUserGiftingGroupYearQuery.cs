using Application.Areas.Participate.BaseModels;
using Application.Areas.Participate.Mapping;
using Global.Abstractions.Areas.Participate;
using Global.Extensions.Exceptions;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Areas.Participate.Queries;

public sealed class ManageUserGiftingGroupYearQuery : BaseQuery<IManageUserGiftingGroupYear>
{
    public int GiftingGroupKey { get; }
    public int CalendarYear { get; }

    public ManageUserGiftingGroupYearQuery(int giftingGroupKey, int? calendarYear = null)
    {
        GiftingGroupKey = giftingGroupKey;
        CalendarYear = calendarYear ?? DateTime.Today.Year;
    }

    protected override Task<IManageUserGiftingGroupYear> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);
        IQueryable<IUserGiftingGroupYear> userGroups = new List<IUserGiftingGroupYear>().AsQueryable();

        ICollection<Santa_GiftingGroupUser> dbGroupLinks = dbCurrentSantaUser.GiftingGroupLinks;

        if (dbGroupLinks?.Any() != true)
            throw new NotFoundException("Gifting Group");

        var dbActiveLinks = dbGroupLinks
                .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null);

        Santa_GiftingGroupUser? dbGiftingGroupLink = dbCurrentSantaUser.GiftingGroupLinks
            .FirstOrDefault(x => x.GiftingGroupKey == GiftingGroupKey);

        if (dbGiftingGroupLink == null)
            throw new NotFoundException("Gifting Group");

        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;

        Santa_GiftingGroupYear? dbYear = dbGiftingGroup.Years
            .Where(x => x.CalendarYear == CalendarYear)
            .FirstOrDefault();

        Santa_YearGroupUser? dbYearGroupUser = dbYear?.Users.FirstOrDefault(u => u.SantaUserKey == dbCurrentSantaUser.SantaUserKey);

        IManageUserGiftingGroupYear? manageYear = dbYearGroupUser?.ToManageUserGiftingGroupYear(Mapper);

        if (manageYear == null) // not created yet
        {
            manageYear = new ManageUserGiftingGroupYear
            {
                GiftingGroupKey = dbGiftingGroupLink.GiftingGroupKey,
                GiftingGroupName = dbGiftingGroup.Name,
                MemberStatus = dbGiftingGroupLink.GroupAdmin ? GroupMemberStatus.Admin : GroupMemberStatus.Joined,
                Limit = dbYear?.Limit,
                CurrencyCode = dbYear?.CurrencyCode ?? dbGiftingGroup.GetCurrencyCode(),
                CurrencySymbol = dbYear?.CurrencySymbol ?? dbGiftingGroup.GetCurrencySymbol(),
                CalendarYear = CalendarYear
            };

            ParticipateManualMappings.SetPreviousYearDetails(manageYear, dbCurrentSantaUser, dbGiftingGroup, Mapper);
        }

        return Result(manageYear);
    }
}
