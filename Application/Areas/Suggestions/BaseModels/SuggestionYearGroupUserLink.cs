using Global.Abstractions.Areas.Suggestions;

namespace Application.Areas.Suggestions.BaseModels;

public class SuggestionYearGroupUserLink : ISuggestionYearGroupUserLink
{
    public int SuggestionLinkKey { get; set; }
    public int YearGroupUserKey { get; set; }
    public string GiftingGroupName { get; set; } = string.Empty;
    public bool Included { get; set; }
}
