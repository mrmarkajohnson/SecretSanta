namespace Global.Abstractions.Areas.Suggestions;

public interface IManageSuggestion : ISuggestionBase
{
    IEnumerable<IManageSuggestionLink> YearGroupUserLinks { get; }
}
