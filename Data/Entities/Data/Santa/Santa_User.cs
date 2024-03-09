using Data.Entities.Data.Global;

namespace Data.Entities.Data.Santa;

public class Santa_User : DeletableBaseEntity, IDeletableEntity
{
    public Santa_User()
    {
        Partner1Links = new HashSet<Santa_PartnerLink>();
        Partner2Links = new HashSet<Santa_PartnerLink>();
        GiftingGroupLinks = new HashSet<Santa_GiftingGroupUser>();
        GiftingGroupYears = new HashSet<Santa_YearGroupUser>();
    }

    [Key]
    public int Id { get; set; }

    public required string GlobalUserId { get; set; }
    public virtual required Global_User GlobalUser { get; set; }

    //[Required, Length(10, 30)]
    //public string UserName { get; set; } = "";    

    //public string? EmailAddress { get; set; } = "";

    public virtual ICollection<Santa_PartnerLink> Partner1Links { get; set; } // Relationships suggested by this user
    public virtual ICollection<Santa_PartnerLink> Partner2Links { get; set; } // Relationships suggested by another user, confirmed by this user

    public virtual ICollection<Santa_GiftingGroupUser> GiftingGroupLinks { get; set; }
    public virtual ICollection<Santa_YearGroupUser> GiftingGroupYears { get; set; }
}
