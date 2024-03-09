namespace Data.Entities.Data.Santa;

public class Santa_Suggestion : ArchivableBaseEntity, IArchivableEntity
{
    [Key]
    public int Id { get; set; }

    public int SuggesterId { get; set; } // The person who makes the suggestion
    public virtual required Santa_YearGroupUser Suggester { get; set; }

    public bool MainSuggestion { get; set; }

    public required string SuggestionText { get; set; }

    public required string OtherNotes { get; set; } // E.g. things to avoid
}
