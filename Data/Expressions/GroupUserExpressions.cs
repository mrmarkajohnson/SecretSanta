using Data.Entities.Santa;

namespace Data.Expressions;

public static class GroupUserExpressions
{
    public static Func<Santa_GiftingGroupUser, bool> IsActive(bool checkGroup)
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
