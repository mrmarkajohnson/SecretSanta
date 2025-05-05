using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Suggestions;

public interface IManageSuggestion : ISuggestionBase, IHasCalendarYear
{
    IEnumerable<IManageSuggestionLink> YearGroupUserLinks { get; }
}
