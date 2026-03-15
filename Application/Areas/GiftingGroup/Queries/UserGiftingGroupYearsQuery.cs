using Application.Areas.Participate.Mapping;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Participate;

namespace Application.Areas.GiftingGroup.Queries;

public sealed class UserGiftingGroupYearsQuery : BaseQuery<IQueryable<IUserGiftingGroupYear>>
{
    private int CalendarYear { get; }

    public UserGiftingGroupYearsQuery(int? calendarYear = null)
    {
        CalendarYear = calendarYear ?? GlobalSettings.CurrentYear;
    }

    protected override Task<IQueryable<IUserGiftingGroupYear>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);
        IQueryable<IUserGiftingGroupYear> userGroups = new List<IUserGiftingGroupYear>().AsQueryable();

        var dbActiveLinks = dbCurrentSantaUser.GiftingGroupLinks
            .Where(GroupUserExpressions.IsActive(true))
            .Where(x => CalendarYear > GlobalSettings.CurrentYear
                ? x.GiftingGroup.DateCreated.Year == CalendarYear
                : x.GiftingGroup.DateCreated.Year <= CalendarYear);

        if (dbActiveLinks?.Any() == true)
        {
            userGroups = GetYearsWithMemberSet(dbCurrentSantaUser, dbActiveLinks)
                .Union(GetYearsWithMemberNotSet(dbCurrentSantaUser, dbActiveLinks))
                .Union(GetJoinerRequests(dbCurrentSantaUser, dbActiveLinks))
                .Union(GetInvites(dbCurrentSantaUser, dbActiveLinks));
        }

        return Result(userGroups);
    }

    private IQueryable<IUserGiftingGroupYear> GetYearsWithMemberSet(Santa_User dbSantaUser, IEnumerable<Santa_GiftingGroupUser> dbActiveLinks)
    {
        IEnumerable<Santa_YearGroupUser> dbYearGroupUsers = dbActiveLinks
            .SelectMany(x => x.GiftingGroup.Years
                .Where(GroupYearExpressions.IsActive(false))
                .Where(x => x.CalendarYear == CalendarYear)
                .SelectMany(y => y.Users.Where(u => u.SantaUserKey == dbSantaUser.SantaUserKey)));

        return dbYearGroupUsers
            .Select(x => (x.ToUserGiftingGroupYear(Mapper, CalendarYear)))
            .AsQueryable();
    }


    private IQueryable<IUserGiftingGroupYear> GetYearsWithMemberNotSet(Santa_User dbSantaUser, IEnumerable<Santa_GiftingGroupUser> dbActiveLinks)
    {
        return dbActiveLinks
            .Where(x => x.GiftingGroup.Years
                .Where(GroupYearExpressions.IsActive(false))
                .Where(x => x.CalendarYear == CalendarYear)
                .Any(y => y.Users.Any(u => u.SantaUserKey == dbSantaUser.SantaUserKey)) == false)
            .AsQueryable()
            .ProjectTo<IUserGiftingGroupYear>(Mapper.ConfigurationProvider, new { CalendarYear });
    }

    private IQueryable<IUserGiftingGroupYear> GetJoinerRequests(Santa_User dbSantaUser, IEnumerable<Santa_GiftingGroupUser> dbActiveLinks)
    {
        IEnumerable<Santa_GiftingGroupApplication> dbJoinerRequests = dbSantaUser.GiftingGroupApplications
            .Where(GroupApplicationExpressions.IsActive(true))
            .Where(x => x.Accepted == null)
            .Where(x => dbActiveLinks.Any(y => y.GiftingGroupKey == x.GiftingGroupKey) == false);

        return dbJoinerRequests
            .AsQueryable()
            .ProjectTo<IUserGiftingGroupYear>(Mapper.ConfigurationProvider, new { CalendarYear });
    }

    private IQueryable<IUserGiftingGroupYear> GetInvites(Santa_User dbSantaUser, IEnumerable<Santa_GiftingGroupUser> dbActiveLinks)
    {
        IEnumerable<Santa_Invitation> dbGroupInvitations = dbSantaUser.ReceivedInvitations
            .Where(GroupInvitationExpressions.IsActive(true))
            .Where(x => dbActiveLinks.Any(y => y.GiftingGroupKey == x.GiftingGroupKey) == false);

        return dbGroupInvitations
            .AsQueryable()
            .ProjectTo<IUserGiftingGroupYear>(Mapper.ConfigurationProvider, new { CalendarYear });
    }
}
