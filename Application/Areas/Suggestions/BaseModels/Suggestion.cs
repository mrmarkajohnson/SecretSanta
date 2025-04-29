using Global.Abstractions.Areas.Suggestions;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Suggestions.BaseModels;

public class Suggestion : SuggestionBase, ISuggestion
{
    public Suggestion()
    {
        YearGroupUserLinks = new List<SuggestionYearGroupUserLink>();
    }

    [Display(Name = "Groups")]
    public IList<SuggestionYearGroupUserLink> YearGroupUserLinks { get; set; }
    IEnumerable<ISuggestionYearGroupUserLink> ISuggestion.YearGroupUserLinks => YearGroupUserLinks;
}
