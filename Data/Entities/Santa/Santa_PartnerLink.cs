namespace Data.Entities.Santa;

public class Santa_PartnerLink : DeletableBaseEntity, IDeletableEntity
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The user who first suggests the relationship
    /// </summary>
    public int SuggestedById { get; set; }
    public virtual required Santa_User SuggestedBy { get; set; } 

    public bool Confirmed { get; set; }

    /// <summary>
    /// The user who confirms the relationship
    /// </summary>
    public int ConfirmedById { get; set; }
    public virtual required Santa_User ConfirmedBy { get; set; } 

    public DateTime? RelationshipEnded { get; set; }

    /// <summary>
    /// 'SuggestedBy' said this is an old relationship that can now be ignored
    /// </summary>
    public bool SuggestedByIgnoreOld { get; set; }

    /// <summary>
    /// 'ConfirmedBy' said this is an old relationship that can now be ignored
    /// </summary>
    public bool ConfirmedByIgnoreOld { get; set; }
}
