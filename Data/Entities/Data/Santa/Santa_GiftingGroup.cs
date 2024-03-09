namespace Data.Entities.Data.Santa;

public class Santa_GiftingGroup : DeletableBaseEntity, IDeletableEntity
{
    public Santa_GiftingGroup()
    {
        UserLinks = new HashSet<Santa_GiftingGroupUser>();
        Years = new HashSet<Santa_GiftingGroupYear>();
    }

    [Key]
    public int Id { get; set; }

    [Required, Length(4, 150)]
    public string Name { get; set; } = "";

    [Required, Length(6, 250)]
    public string Description { get; set; } = "";

    [Required, Length(8, 15)]
    public string JoinerToken { get; set; } = "";

    public virtual ICollection<Santa_GiftingGroupUser> UserLinks { get; set; }
    public virtual ICollection<Santa_GiftingGroupYear> Years { get; set; }
}
