using Global.Abstractions.Areas.Suggestions;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Suggestions.BaseModels;

public class ManageSuggestion : SuggestionBase, IManageSuggestion
{
    public ManageSuggestion()
    {
        YearGroupUserLinks = new List<ManageSuggestionLink>();
    }

    [Display(Name="Groups")]
    public IList<ManageSuggestionLink> YearGroupUserLinks { get; set; }
    IEnumerable<IManageSuggestionLink> IManageSuggestion.YearGroupUserLinks => YearGroupUserLinks;
}
