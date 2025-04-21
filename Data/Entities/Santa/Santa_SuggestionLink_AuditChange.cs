namespace Data.Entities.Santa;

public class Santa_SuggestionLink_AuditChange : AuditBaseChange, IAuditChangeEntity<Santa_SuggestionLink_Audit>
{
    public virtual Santa_SuggestionLink_Audit Audit { get; set; } = new();
}