using Data.Entities.Santa;

namespace Data.Expressions;

public static class GroupApplicationExpressions
{
    public static Func<Santa_GiftingGroupApplication, bool> IsActive(bool checkGroup)
    {
        if (checkGroup)
        {
            return x => x.DateArchived == null && x.SantaUser.DateArchived == null 
                && x.GiftingGroup.DateArchived == null;
        }
        else
        {
            return x => x.DateArchived == null && x.SantaUser.DateArchived == null;
        }
    }
}
