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
        CalendarYear = calendarYear ?? GlobalSettings.CurrentYear;
    }

    protected override Task<IManageUserGiftingGroupYear> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        Santa_GiftingGroupUser? dbGiftingGroupLink = dbCurrentSantaUser.GiftingGroupLinks
            .Where(GroupUserExpressions.IsActive(true))
            .FirstOrDefault(x => x.GiftingGroupKey == GiftingGroupKey);

        if (dbGiftingGroupLink == null)
            throw new NotFoundException("Gifting Group");

        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;

        Santa_GiftingGroupYear? dbYear = dbGiftingGroup.Years
            .Where(GroupYearExpressions.IsActive(false))
            .FirstOrDefault(x => x.CalendarYear == CalendarYear);

        Santa_YearGroupUser? dbYearGroupUser = dbYear?.Users.FirstOrDefault(u => u.SantaUserKey == dbCurrentSantaUser.SantaUserKey);

        IManageUserGiftingGroupYear? manageYear = dbYearGroupUser?.ToManageUserGiftingGroupYear(Mapper, CalendarYear);

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
