using Data.Entities.Santa;

namespace Data.Expressions;

public static class DbSuggestionLinkExpressions
{
    public static Func<Santa_SuggestionLink, bool> IsActive(bool checkGroup)
    {
        if (checkGroup)
        {
            return x => x.DateDeleted == null && x.DateArchived == null 
                && x.YearGroupUser.GiftingGroupYear.DateArchived == null
                && x.YearGroupUser.GiftingGroupYear.GiftingGroup.DateArchived == null;
        }
        else
        {
            return x => x.DateDeleted == null && x.DateArchived == null;
        }
    }
}
