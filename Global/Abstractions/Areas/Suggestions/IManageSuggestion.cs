using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Suggestions;

public interface IManageSuggestion : ISuggestionBase, IHaveACalendarYear
{
    IEnumerable<IManageSuggestionLink> YearGroupUserLinks { get; }
}
