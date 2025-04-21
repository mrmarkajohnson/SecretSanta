namespace Global.Abstractions.Areas.Suggestions;

public interface ISuggestionYearGroupUserLink
{
    int SuggestionLinkKey { get; }
    int YearGroupUserKey { get; }
    int Year { get; }
    string GiftingGroupName { get; }
}