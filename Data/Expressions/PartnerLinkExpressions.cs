using Data.Entities.Santa;
using System.Linq.Expressions;

namespace Data.Expressions;

public static class PartnerLinkExpressions
{
    public static Func<Santa_PartnerLink, bool> IsActive(bool checkEnded)
    {
        if (checkEnded)
        {
            return x => x.DateDeleted == null && x.DateArchived == null && x.RelationshipEnded == null;
        }
        else
        {
            return x => x.DateDeleted == null && x.DateArchived == null;
        }
    }

    public static Func<Santa_PartnerLink, bool> SuggestingUserIsActive()
    {
        return x => x.SuggestedBySantaUser.DateArchived == null;
    }

    public static Func<Santa_PartnerLink, bool> ConfirmingUserIsActive()
    {
        return x => x.ConfirmingSantaUser.DateArchived == null;
    }

    public static Expression<Func<Santa_PartnerLink, IEnumerable<string>>> RelationshipSharedGroupNames()
    {
        return x => x.ConfirmingSantaUser.GiftingGroupLinks
                .Where(GroupUserExpressions.IsActive(true))
                .Where(y => x.SuggestedBySantaUser.GiftingGroupLinks
                    .Where(GroupUserExpressions.IsActive(false))
                    .Any(z => z.GiftingGroupKey == y.GiftingGroupKey))
                .Select(x => x.GiftingGroup.Name);
    }
}
