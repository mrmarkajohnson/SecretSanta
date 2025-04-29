namespace Global.Abstractions.Areas.Suggestions;

public interface ISuggestion : ISuggestionBase
{
    IEnumerable<ISuggestionYearGroupUserLink> YearGroupUserLinks { get; }
}
