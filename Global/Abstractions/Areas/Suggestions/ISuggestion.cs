using Global.Abstractions.Areas.GiftingGroup;

namespace Global.Abstractions.Areas.Suggestions;

public interface ISuggestion : ISuggestionBase
{
    IEnumerable<ISuggestionYearGroupUserLink> YearGroupUserLinks { get; }
}

public static class SuggestionExtensions
{   
    public static bool AppliesToGroup(this ISuggestion suggestion, IUserGiftingGroup group)
    {
        return suggestion.YearGroupUserLinks.Any(y => y.GiftingGroupName == group.GroupName);
    }
}
