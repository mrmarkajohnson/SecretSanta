namespace Data.Entities.Santa;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class Santa_GiftingGroupYear_Audit : AuditBaseEntity, IAuditEntity<Santa_GiftingGroupYear, Santa_GiftingGroupYear_AuditChange>
{
    public Santa_GiftingGroupYear_Audit()
    {
        Changes = new HashSet<Santa_GiftingGroupYear_AuditChange>();
    }

    public int ParentKey { get; set; }
    public virtual Santa_GiftingGroupYear Parent { get; set; }

    public virtual ICollection<Santa_GiftingGroupYear_AuditChange> Changes { get; set; }
}