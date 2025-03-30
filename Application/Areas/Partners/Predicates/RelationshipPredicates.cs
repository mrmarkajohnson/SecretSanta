using System.Linq.Expressions;

namespace Application.Areas.Partners.Predicates;

public static class RelationshipPredicates
{
    public static Expression<Func<Santa_PartnerLink, IEnumerable<string>>> RelationshipSharedGroupNames()
    {
        return src =>
            src.ConfirmingSantaUser.GiftingGroupLinks
                .Where(x => x.DateArchived == null && x.DateDeleted == null)
                .Where(x => x.GiftingGroup.DateArchived == null && x.GiftingGroup.DateDeleted == null)
                .Where(x => src.SuggestedBySantaUser.GiftingGroupLinks
                    .Where(y => y.DateArchived == null && y.DateDeleted == null)
                    .Any(y => y.GiftingGroupKey == x.GiftingGroupKey))
                .Select(x => x.GiftingGroup.Name);
    }
}
