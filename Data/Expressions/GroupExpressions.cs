using Data.Entities.Santa;

namespace Data.Expressions;

public static class GroupExpressions
{
    public static Func<Santa_GiftingGroup, bool> IsActive()
    {
        return x => x.DateArchived == null;
    }
}
