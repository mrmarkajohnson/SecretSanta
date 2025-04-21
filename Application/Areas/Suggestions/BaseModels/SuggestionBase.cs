using Global.Abstractions.Areas.Suggestions;

namespace Application.Areas.Suggestions.BaseModels;

public class SuggestionBase : ISuggestionBase
{
    public int SuggestionKey { get; set; }
    public int SantaUserKey { get; set; }
    public int Priority { get; set; }
    public required string SuggestionText { get; set; }
    public required string OtherNotes { get; set; }
}
