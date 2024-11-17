using Global.Validation;

namespace Data.Entities.Santa;

public class Santa_GiftingGroupApplication : DeletableBaseEntity, IDeletableEntity
{
    [Key]
    public int Id { get; set; }

    public int SantaUserId { get; set; }
    public virtual required Santa_User SantaUser { get; set; }

    public int GiftingGroupId { get; set; }
    public virtual required Santa_GiftingGroup GiftingGroup { get; set; }

    public int? ResponseByUserId { get; set; }
    public virtual Santa_User? ResponseByUser { get; set; }

    [MaxLength(GiftingGroupVal.JoinerMessage.MaxLength)]
    public string? Message { get; set; }

    public bool? Accepted { get; set; }
    public string? RejectionMessage { get; set; }
    public bool Blocked { get; set; }
}
