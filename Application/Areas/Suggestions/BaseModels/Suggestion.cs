using Global.Abstractions.Areas.Suggestions;

namespace Application.Areas.Suggestions.BaseModels;

public class Suggestion : SuggestionBase, ISuggestion
{
    public Suggestion()
    {
        YearGroupUserLinks = new List<SuggestionYearGroupUserLink>();
    }

    public IList<SuggestionYearGroupUserLink> YearGroupUserLinks { get; set; }
    IEnumerable<ISuggestionYearGroupUserLink> ISuggestion.YearGroupUserLinks => YearGroupUserLinks;
}
