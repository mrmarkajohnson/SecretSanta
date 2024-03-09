namespace Data.Entities.Data.Santa;

public class Santa_PartnerLink : DeletableBaseEntity, IDeletableEntity
{
    [Key]
    public int Id { get; set; }

    public int Partner1Id { get; set; }
    public virtual required Santa_User Partner1 { get; set; } // The user who first suggests the relationship

    public int Partner2Id { get; set; }
    public virtual required Santa_User Partner2 { get; set; } // The user who confirms the relationship

    public bool ConfirmedByPartner2 { get; set; }

    public DateTime? RelationshipEnded { get; set; }
    public bool? IgnoreOldRelationship { get; set; }
}
