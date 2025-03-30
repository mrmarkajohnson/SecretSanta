namespace Data.Entities.Santa;

public class Santa_GiftingGroupUser : DeletableBaseEntity, IDeletableEntity
{
    [Key]
    public int GiftingGroupUserKey { get; set; }

    public bool GroupAdmin { get; set; }

    public int SantaUserKey { get; set; }
    public virtual required Santa_User SantaUser { get; set; }

    public int GiftingGroupKey { get; set; }
    public virtual required Santa_GiftingGroup GiftingGroup { get; set; }
}
