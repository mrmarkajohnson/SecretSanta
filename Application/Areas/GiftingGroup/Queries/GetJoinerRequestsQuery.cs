using Application.Areas.Account.Actions;
using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Queries;

public class GetJoinerRequestsQuery : BaseQuery<IQueryable<IReviewApplication>>
{
    public GetJoinerRequestsQuery()
    {
    }

    protected async override Task<IQueryable<IReviewApplication>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var dbApplications = dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null && x.GroupAdmin)
            .Select(x => x.GiftingGroup)
            .SelectMany(x => x.MemberApplications)
            .Where(y => y.ResponseByUserId == null)
            .AsQueryable();

        var applications = dbApplications.ProjectTo<IReviewApplication>(Mapper.ConfigurationProvider).ToList();

        foreach (var application in applications)
        {
            await Send(new UnHashUserIdentificationAction(application));
        }

        return applications.AsQueryable();
    }
}
