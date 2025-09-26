using Application.Areas.Participate.Mapping;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Participate;

namespace Application.Areas.GiftingGroup.Queries;

public sealed class UserGiftingGroupYearsQuery : BaseQuery<IQueryable<IUserGiftingGroupYear>>
{
    protected override Task<IQueryable<IUserGiftingGroupYear>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);
        IQueryable<IUserGiftingGroupYear> userGroups = new List<IUserGiftingGroupYear>().AsQueryable();

        ICollection<Santa_GiftingGroupUser> dbGroupLinks = dbCurrentSantaUser.GiftingGroupLinks;

        if (dbGroupLinks?.Any() == true)
        {
            var dbActiveLinks = dbGroupLinks
                .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null);

            userGroups = GetYearsWithMemberSet(dbCurrentSantaUser, dbActiveLinks)
                .Union(GetYearsWithMemberNotSet(dbCurrentSantaUser, dbActiveLinks))
                .Union(GetJoinerRequests(dbCurrentSantaUser, dbActiveLinks));
        }

        return Result(userGroups);
    }

    private IQueryable<IUserGiftingGroupYear> GetYearsWithMemberSet(Santa_User dbSantaUser, IEnumerable<Santa_GiftingGroupUser> dbActiveLinks)
    {
        IEnumerable<Santa_YearGroupUser> dbYearGroupUsers = dbActiveLinks
            .SelectMany(x => x.GiftingGroup.Years
                .Where(x => x.CalendarYear == DateTime.Today.Year)
                .SelectMany(y => y.Users.Where(u => u.SantaUserKey == dbSantaUser.SantaUserKey)));

        return dbYearGroupUsers
            .Select(x => (x.ToUserGiftingGroupYear(Mapper)))
            .AsQueryable();
    }


    private IQueryable<IUserGiftingGroupYear> GetYearsWithMemberNotSet(Santa_User dbSantaUser, IEnumerable<Santa_GiftingGroupUser> dbActiveLinks)
    {
        return dbActiveLinks
            .Where(x => x.GiftingGroup.Years.Where(x => x.CalendarYear == DateTime.Today.Year)
                .Any(y => y.Users.Any(u => u.SantaUserKey == dbSantaUser.SantaUserKey)) == false)
            .AsQueryable()
            .ProjectTo<IUserGiftingGroupYear>(Mapper.ConfigurationProvider);
    }

    private IQueryable<IUserGiftingGroupYear> GetJoinerRequests(Santa_User dbSantaUser, IEnumerable<Santa_GiftingGroupUser> dbActiveLinks)
    {
        IEnumerable<Santa_GiftingGroupApplication> dbJoinerRequests = dbSantaUser.GiftingGroupApplications
            .Where(x => x.DateDeleted == null && x.DateArchived == null)
            .Where(x => x.Accepted == null)
            .Where(x => dbActiveLinks.Any(y => y.GiftingGroupKey == x.GiftingGroupKey) == false);

        return dbJoinerRequests
            .AsQueryable()
            .ProjectTo<IUserGiftingGroupYear>(Mapper.ConfigurationProvider);
    }
}
