namespace Data.Entities.Santa;

public class Santa_PartnerLink : DeletableBaseEntity, IDeletableEntity
{
    [Key]
    public int PartnerLinkKey { get; set; }

    /// <summary>
    /// The user who first suggests the relationship
    /// </summary>
    public int SuggestedBySantaUserKey { get; set; }
    public virtual required Santa_User SuggestedBySantaUser { get; set; } 

    public bool Confirmed { get; set; }

    /// <summary>
    /// The user who confirms (or is yet to confirm) the relationship
    /// </summary>
    public int ConfirmingSantaUserKey { get; set; }
    public virtual required Santa_User ConfirmingSantaUser { get; set; } 

    public DateTime? RelationshipEnded { get; set; }

    /// <summary>
    /// 'SuggestedBySantaUser' said this is an old relationship that can now be ignored
    /// </summary>
    public bool SuggestedByIgnoreOld { get; set; }

    /// <summary>
    /// 'ConfirmingSantaUser' said this is an old relationship that can now be ignored
    /// </summary>
    public bool ConfirmedByIgnoreOld { get; set; }
}
