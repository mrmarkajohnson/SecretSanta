using AutoMapper.QueryableExtensions;

namespace Application.Areas.Partners.Queries;

public sealed class GetPossiblePartnersQuery : BaseQuery<IQueryable<IVisibleUser>>
{
    protected override Task<IQueryable<IVisibleUser>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var visibleUsers = dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.DateArchived == null)
            .SelectMany(x => x.GiftingGroup.Members)
            .Where(y => y.DateArchived == null)
            .Where(y => y.SantaUser != null && y.SantaUser.DateArchived == null && y.SantaUserKey != dbCurrentSantaUser.SantaUserKey)
            .Select(y => y.SantaUser)
            .Where(z => z.SuggestedRelationships
                .Where(DbPartnerLinkExpressions.IsActive())
                .Any(r => r.ConfirmingSantaUserKey == dbCurrentSantaUser.SantaUserKey && r.RelationshipEnded == null) == false)
            .Where(z => z.ConfirmingRelationships
                .Where(DbPartnerLinkExpressions.IsActive())
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
