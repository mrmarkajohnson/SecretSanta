namespace Data.Entities.Data.Santa;

public class Santa_PartnerLink : DeletableBaseEntity, IDeletableEntity
{
    [Key]
    public int Id { get; set; }

    public int SuggestedById { get; set; }
    public virtual required Santa_User SuggestedBy { get; set; } // The user who first suggests the relationship

    public int ConfirmedById { get; set; }
    public virtual required Santa_User ConfirmedBy { get; set; } // The user who confirms the relationship

    public bool ConfirmedByPartner2 { get; set; }

    public DateTime? RelationshipEnded { get; set; }
    public bool? IgnoreOldRelationship { get; set; }
}
