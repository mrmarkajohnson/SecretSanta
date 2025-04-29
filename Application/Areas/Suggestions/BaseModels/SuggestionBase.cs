using Global.Abstractions.Areas.Suggestions;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Suggestions.BaseModels;

public class SuggestionBase : ISuggestionBase
{
    public int SuggestionKey { get; set; }
    public int SantaUserKey { get; set; }
    public int Priority { get; set; }

    [Display(Name = "Suggestion"), MaxLength(SuggestionVal.Suggestion.MaxLength)]
    public string SuggestionText { get; set; } = string.Empty;

    [Display(Name = "Notes")]
    public string OtherNotes { get; set; } = string.Empty;
}
