namespace Data.Entities.Data.Santa;

public class Santa_GiftingGroupUser : DeletableBaseEntity, IDeletableEntity
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public virtual required Santa_User User { get; set; }

    public int GiftingGroupId { get; set; }
    public virtual required Santa_GiftingGroup GiftingGroup { get; set; }
}
