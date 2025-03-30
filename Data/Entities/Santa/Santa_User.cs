using Data.Entities.Shared;

namespace Data.Entities.Santa;

public class Santa_User : DeletableBaseEntity, IDeletableEntity
{
    public Santa_User()
    {
        SuggestedRelationships = new HashSet<Santa_PartnerLink>();
        ConfirmingRelationships = new HashSet<Santa_PartnerLink>();
        GiftingGroupLinks = new HashSet<Santa_GiftingGroupUser>();
        GiftingGroupYears = new HashSet<Santa_YearGroupUser>();
        RecipientYears = new HashSet<Santa_YearGroupUser>();
        GiftingGroupApplications = new HashSet<Santa_GiftingGroupApplication>();
        GiftingGroupApplicationResponses = new HashSet<Santa_GiftingGroupApplication>();
        SentMessages = new HashSet<Santa_Message>();
        ReceivedMessages = new HashSet<Santa_MessageRecipient>();
    }

    [Key]
    public int SantaUserKey { get; set; }

    public required string GlobalUserId { get; set; }
    public virtual required Global_User GlobalUser { get; set; }

    /// <summary>
    /// Relationships suggested by this user
    /// </summary>
    public virtual ICollection<Santa_PartnerLink> SuggestedRelationships { get; set; }

    /// <summary>
    /// Relationships suggested by another user, (to be) confirmed by this user
    /// </summary>
    public virtual ICollection<Santa_PartnerLink> ConfirmingRelationships { get; set; }

    public virtual ICollection<Santa_GiftingGroupUser> GiftingGroupLinks { get; set; }
    public virtual ICollection<Santa_YearGroupUser> GiftingGroupYears { get; set; }
    public virtual ICollection<Santa_YearGroupUser> RecipientYears { get; set; }
    public virtual ICollection<Santa_GiftingGroupApplication> GiftingGroupApplications { get; set; }
    public virtual ICollection<Santa_GiftingGroupApplication> GiftingGroupApplicationResponses { get; set; }

    public virtual ICollection<Santa_Message> SentMessages { get; set; }
    public virtual ICollection<Santa_MessageRecipient> ReceivedMessages { get; set; }
}
