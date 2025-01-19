using Application.Areas.Account.Actions;
using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Extensions.Exceptions;

namespace Application.Areas.Partners.Queries;

public class GetPossiblePartnersQuery : BaseQuery<IQueryable<IVisibleUser>>
{
    protected async override Task<IQueryable<IVisibleUser>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var groupNames = dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.DateArchived == null && x.DateDeleted == null)
            .Select(x => x.GiftingGroup.Name)
            .ToList();

        var visibleUsers =dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.DateArchived == null && x.DateDeleted == null)
            .SelectMany(x => x.GiftingGroup.UserLinks)
            .Where(y => y.DateArchived == null && y.DateDeleted == null)
            .Where(y => y.SantaUser != null && y.SantaUserId != dbCurrentSantaUser.Id)
            .Select(y => y.SantaUser)
            .Where(z => z.SuggestedRelationships
                .Where(r => r.DateArchived == null && r.DateDeleted == null)
                .Any(r => r.ConfirmingSantaUserId == dbCurrentSantaUser.Id && r.RelationshipEnded == null) == false)
            .Where(z => z.ConfirmingRelationships
                .Where(r => r.DateArchived == null && r.DateDeleted == null)
                .Any(r => r.SuggestedBySantaUserId == dbCurrentSantaUser.Id && r.RelationshipEnded == null) == false)
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
