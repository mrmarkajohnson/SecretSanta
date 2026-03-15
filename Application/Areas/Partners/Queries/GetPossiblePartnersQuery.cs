using AutoMapper.QueryableExtensions;

namespace Application.Areas.Partners.Queries;

public sealed class GetPossiblePartnersQuery : BaseQuery<IQueryable<IVisibleUser>>
{
    protected override Task<IQueryable<IVisibleUser>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var visibleUsers = dbCurrentSantaUser.GiftingGroupLinks
            .Where(GroupUserExpressions.IsActive(true))
            .SelectMany(x => x.GiftingGroup.Members)
                .Where(GroupUserExpressions.IsActive(false))
                .Where(y => y.SantaUserKey != dbCurrentSantaUser.SantaUserKey)
                .Select(y => y.SantaUser)
                    .Where(z => z.SuggestedRelationships
                        .Where(PartnerLinkExpressions.IsActive(true))
                        .Where(PartnerLinkExpressions.ConfirmingUserIsActive())
                        .Any(r => r.ConfirmingSantaUserKey == dbCurrentSantaUser.SantaUserKey) == false)
                    .Where(z => z.ConfirmingRelationships
                        .Where(PartnerLinkExpressions.IsActive(true))
                        .Where(PartnerLinkExpressions.SuggestingUserIsActive())
                        .Any(r => r.SuggestedBySantaUserKey == dbCurrentSantaUser.SantaUserKey) == false)
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
