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

public static class SuggestionBaseExtensions
{
    public static string NotesTruncated(this ISuggestionBase suggestion)
    {
        int maxLength = 100;

        if (suggestion.OtherNotes == null)
        {
            return string.Empty;
        }
        else if (suggestion.OtherNotes.Length <= maxLength)
        {
            return suggestion.OtherNotes;
        }
        else
        {
            return suggestion.OtherNotes.Substring(0, maxLength - 3) + "...";
        }
    }
}
