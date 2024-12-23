using Application.Santa.Areas.Account.Actions;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.GiftingGroup.Queries;

public class GetJoinerRequestsQuery : BaseQuery<IQueryable<IReviewApplication>>
{
    public GetJoinerRequestsQuery()
    {
    }

    protected async override Task<IQueryable<IReviewApplication>> Handle()
    {
        Global_User? dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);
        if (dbCurrentUser == null || dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        var dbApplications = dbCurrentUser.SantaUser?.GiftingGroupLinks
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
