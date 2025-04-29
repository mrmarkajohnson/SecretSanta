using Global.Abstractions.Areas.Suggestions;

namespace Application.Areas.Suggestions.BaseModels;

public class SuggestionYearGroupUserLink : ISuggestionYearGroupUserLink
{
    public int SuggestionLinkKey { get; set; }
    public int YearGroupUserKey { get; set; }
    public required string GiftingGroupName { get; set; }
    public bool Included { get; set; }
}
