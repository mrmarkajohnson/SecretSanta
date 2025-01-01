using Application.Santa.Areas.Account.Actions;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Global.Shared;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.Partners.Queries;

public class GetPossiblePartnersQuery : BaseQuery<IQueryable<IVisibleUser>>
{
    protected async override Task<IQueryable<IVisibleUser>> Handle()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);
        if (dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        var groupNames = dbCurrentUser.SantaUser.GiftingGroupLinks
            .Where(x => x.DateArchived == null && x.DateDeleted == null)
            .Select(x => x.GiftingGroup.Name)
            .ToList();

        var visibleUsers = dbCurrentUser.SantaUser.GiftingGroupLinks
            .Where(x => x.DateArchived == null && x.DateDeleted == null)
            .SelectMany(x => x.GiftingGroup.UserLinks)
            .Where(y => y.DateArchived == null && y.DateDeleted == null)
            .Where(y => y.SantaUser != null && y.SantaUserId != dbCurrentUser.SantaUser.Id)
            .Select(y => y.SantaUser)
            .Where(z => z.SuggestedRelationships
                .Where(r => r.DateArchived == null && r.DateDeleted == null)
                .Any(r => r.ConfirmingSantaUserId == dbCurrentUser.SantaUser.Id && r.RelationshipEnded == null) == false)
            .Where(z => z.ConfirmingRelationships
                .Where(r => r.DateArchived == null && r.DateDeleted == null)
                .Any(r => r.SuggestedBySantaUserId == dbCurrentUser.SantaUser.Id && r.RelationshipEnded == null) == false)
            .Select(z => z.GlobalUser)
            .DistinctBy(g => g.Id)
            .AsQueryable()
            .ProjectTo<IVisibleUser>(Mapper.ConfigurationProvider, new { GroupNames = groupNames })
            .ToList();

        foreach (var user in visibleUsers)
        {
            await Send(new UnHashUserIdentificationAction(user));
        }

        return visibleUsers.AsQueryable();
    }
}
