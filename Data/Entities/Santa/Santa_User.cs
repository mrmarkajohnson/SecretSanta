using Data.Entities.Shared;

namespace Data.Entities.Santa;

public class Santa_User : DeletableBaseEntity, IDeletableEntity
{
    public Santa_User()
    {
        SuggestedRelationships = new HashSet<Santa_PartnerLink>();
        ConfirmedRelationships = new HashSet<Santa_PartnerLink>();
        GiftingGroupLinks = new HashSet<Santa_GiftingGroupUser>();
        GiftingGroupYears = new HashSet<Santa_YearGroupUser>();
    }

    [Key]
    public int Id { get; set; }

    public required string GlobalUserId { get; set; }
    public virtual required Global_User GlobalUser { get; set; }

    public virtual ICollection<Santa_PartnerLink> SuggestedRelationships { get; set; } // Relationships suggested by this user
    public virtual ICollection<Santa_PartnerLink> ConfirmedRelationships { get; set; } // Relationships suggested by another user, confirmed by this user

    public virtual ICollection<Santa_GiftingGroupUser> GiftingGroupLinks { get; set; }
    public virtual ICollection<Santa_YearGroupUser> GiftingGroupYears { get; set; }
}
