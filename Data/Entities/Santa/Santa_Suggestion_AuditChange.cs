namespace Data.Entities.Santa;

public class Santa_Suggestion_AuditChange : AuditBaseChange, IAuditChangeEntity<Santa_Suggestion_Audit>
{
    public virtual Santa_Suggestion_Audit Audit { get; set; } = new();
}
