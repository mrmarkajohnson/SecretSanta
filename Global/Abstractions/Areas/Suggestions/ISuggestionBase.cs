namespace Global.Abstractions.Areas.Suggestions;

public interface ISuggestionBase
{
    int SuggestionKey { get; }
    int SantaUserKey { get; }

    int Priority { get; }
    string SuggestionText { get; }

    /// <summary>
    /// E.g. things to avoid
    /// </summary>
    string OtherNotes { get; }
}
