using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Queries;

public sealed class GetJoinerRequestsQuery : BaseQuery<IQueryable<IReviewApplication>>
{
    public GetJoinerRequestsQuery()
    {
    }

    protected override Task<IQueryable<IReviewApplication>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);
        object parameters = new { CurrentSantaUserKey = dbCurrentSantaUser.SantaUserKey, UserKeysForVisibleEmail = dbCurrentSantaUser.UserKeysForVisibleEmail() };

        var dbApplications = dbCurrentSantaUser.GiftingGroupLinks
            .Where(GroupUserExpressions.IsActive(true))
            .Where(x => x.GroupAdmin)
            .Select(x => x.GiftingGroup)
                .SelectMany(y => y.MemberApplications)
                    .Where(GroupApplicationExpressions.IsActive(false))
                    .Where(z => z.ResponseBySantaUserKey == null)
                    .AsQueryable();

        var applications = dbApplications.ProjectTo<IReviewApplication>(Mapper.ConfigurationProvider, parameters).ToList();
        applications.ForEach(x => x.UnHash());

        return Result(applications.AsQueryable());
    }
}
