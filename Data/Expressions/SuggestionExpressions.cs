using Data.Entities.Santa;

namespace Data.Expressions;

public static class SuggestionExpressions
{
    public static Func<Santa_Suggestion, bool> IsActive()
    {
        return x => x.DateDeleted == null && x.DateArchived == null;
    }
}
