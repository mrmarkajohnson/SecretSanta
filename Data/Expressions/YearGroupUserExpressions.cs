using Data.Entities.Santa;

namespace Data.Expressions;

public static class YearGroupUserExpressions
{

    public static Func<Santa_YearGroupUser, bool> IsActive(bool checkUser)
    {
        if (checkUser)
        {
            return x => x.GiftingGroupYear.DateArchived == null && x.SantaUser.DateArchived == null;
        }
        else
        {
            return x => x.GiftingGroupYear.DateArchived == null;
        }
    }
}
