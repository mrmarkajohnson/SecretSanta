using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;

namespace Application.Areas.Partners.Queries;

public sealed class GetPossiblePartnersQuery : BaseQuery<IQueryable<IVisibleUser>>
{
    protected override Task<IQueryable<IVisibleUser>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var visibleUsers = dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.DateArchived == null && x.DateDeleted == null)
            .SelectMany(x => x.GiftingGroup.UserLinks)
            .Where(y => y.DateArchived == null && y.DateDeleted == null)
            .Where(y => y.SantaUser != null && y.SantaUserKey != dbCurrentSantaUser.SantaUserKey)
            .Select(y => y.SantaUser)
            .Where(z => z.SuggestedRelationships
                .Where(r => r.DateArchived == null && r.DateDeleted == null)
                .Any(r => r.ConfirmingSantaUserKey == dbCurrentSantaUser.SantaUserKey && r.RelationshipEnded == null) == false)
            .Where(z => z.ConfirmingRelationships
                .Where(r => r.DateArchived == null && r.DateDeleted == null)
                .Any(r => r.SuggestedBySantaUserKey == dbCurrentSantaUser.SantaUserKey && r.RelationshipEnded == null) == false)
            .Select(z => z.GlobalUser)
            .DistinctBy(g => g.Id)
            .AsQueryable()
            .ProjectTo<IVisibleUser>(Mapper.ConfigurationProvider,
                new { GroupNames = dbCurrentSantaUser.GroupNames(), UserKeysForVisibleEmail = dbCurrentSantaUser.UserKeysForVisibleEmail() })
            .ToList();

        visibleUsers.ForEach(x => x.UnHash());

        return Result(visibleUsers.AsQueryable());
    }
}
