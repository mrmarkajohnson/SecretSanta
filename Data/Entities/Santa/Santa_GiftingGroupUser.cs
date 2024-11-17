namespace Data.Entities.Santa;

public class Santa_GiftingGroupUser : DeletableBaseEntity, IDeletableEntity
{
    public Santa_GiftingGroupUser()
    {
        ReceivedMessages = new HashSet<Santa_MessageRecipient>();
    }

    [Key]
    public int Id { get; set; }

    public bool GroupAdmin { get; set; }

    public int SantaUserId { get; set; }
    public virtual required Santa_User SantaUser { get; set; }

    public int GiftingGroupId { get; set; }
    public virtual required Santa_GiftingGroup GiftingGroup { get; set; }

    public virtual ICollection<Santa_MessageRecipient> ReceivedMessages { get; set; }
}
