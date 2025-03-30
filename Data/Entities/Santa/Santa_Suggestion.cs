namespace Data.Entities.Santa;

public class Santa_Suggestion : ArchivableBaseEntity, IArchivableEntity
{
    [Key]
    public int SuggestionKey { get; set; }

    /// <summary>
    /// Identifies the group member who makes the suggestion
    /// </summary>
    public int YearGroupUserKey { get; set; }

    /// <summary>
    /// Identifies the group member who makes the suggestion
    /// </summary>
    public virtual required Santa_YearGroupUser YearGroupUser { get; set; }

    public bool MainSuggestion { get; set; }

    public required string SuggestionText { get; set; }

    /// <summary>
    /// E.g. things to avoid
    /// </summary>
    public required string OtherNotes { get; set; }
}
