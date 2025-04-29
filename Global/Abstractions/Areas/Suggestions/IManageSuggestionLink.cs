using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Suggestions;

public interface IManageSuggestionLink : ISuggestionYearGroupUserLink, IHasCalendarYear
{
    /// <summary>
    /// Apply the suggestion to this group in this year
    /// </summary>
    bool ApplyToGroup { get; }

    int GiftingGroupKey { get; }
}