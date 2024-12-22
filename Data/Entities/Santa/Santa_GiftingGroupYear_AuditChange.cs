namespace Data.Entities.Santa;

public class Santa_GiftingGroupYear_AuditChange : AuditBaseChange, IAuditChangeEntity<Santa_GiftingGroupYear_Audit>
{
    public virtual Santa_GiftingGroupYear_Audit Audit { get; set; } = new();
}
