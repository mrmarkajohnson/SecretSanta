using Global.Abstractions.Areas.Suggestions;

namespace Application.Areas.Suggestions.BaseModels;

public class ManageSuggestionLink : SuggestionYearGroupUserLink, IManageSuggestionLink
{
    public int GiftingGroupKey { get; set; }
    public int CalendarYear { get; set; }
    public bool ApplyToGroup { get; set; }
}