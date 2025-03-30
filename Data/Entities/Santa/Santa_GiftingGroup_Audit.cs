namespace Data.Entities.Santa;

public class Santa_GiftingGroup_Audit : AuditBaseEntity, IAuditEntity<Santa_GiftingGroup, Santa_GiftingGroup_AuditChange>
{
    public Santa_GiftingGroup_Audit()
    {
        Changes = new HashSet<Santa_GiftingGroup_AuditChange>();
    }
    
    public int ParentKey { get; set; }
    public virtual Santa_GiftingGroup Parent { get; set; } = new();

    public virtual ICollection<Santa_GiftingGroup_AuditChange> Changes { get; set; }
}
