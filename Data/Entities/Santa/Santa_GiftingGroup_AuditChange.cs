namespace Data.Entities.Santa;

public class Santa_GiftingGroup_AuditChange : AuditBaseChange, IAuditChangeEntity<Santa_GiftingGroup_Audit>
{
    public virtual Santa_GiftingGroup_Audit Audit { get; set; } = new();
}
