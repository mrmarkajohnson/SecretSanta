using Data.Entities.Santa;

namespace Data.Expressions;

public static class GroupYearExpressions
{
    public static Func<Santa_GiftingGroupYear, bool> IsActive(bool checkGroup)
    {
        if (checkGroup)
        {
            return x => x.DateArchived == null && x.GiftingGroup.DateArchived == null;
        }
        else
        {
            return x => x.DateArchived == null;
        }
    }
}
