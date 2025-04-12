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

    /// <summary>
    /// Null means awaiting confirmation, false means never in a relationship
    /// </summary>
    public bool? Confirmed { get; set; }

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
    /// 'ConfirmingSantaUser' said this is an old relationship (or not a relationship) but can be ignored
    /// </summary>
    public bool ConfirmingUserIgnore { get; set; }

    /// <summary>
    /// Include in the gift exchange 'draw' if true, exclude if false
    /// Allows users to permanently block an old relationship, or someone inappropriately suggesting relationships
    /// </summary>
    public bool ExchangeGifts { get; set; }
}
