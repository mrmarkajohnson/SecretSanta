using Global.Validation;

namespace Data.Entities.Santa;

public class Santa_GiftingGroupApplication : DeletableBaseEntity, IDeletableEntity
{
    [Key]
    public int GroupApplicationKey { get; set; }

    public int SantaUserKey { get; set; }
    public virtual required Santa_User SantaUser { get; set; }

    public int GiftingGroupKey { get; set; }
    public virtual required Santa_GiftingGroup GiftingGroup { get; set; }

    public int? ResponseBySantaUserKey { get; set; }
    public virtual Santa_User? ResponseBySantaUser { get; set; }

    [MaxLength(GiftingGroupVal.JoinerMessage.MaxLength)]
    public string? Message { get; set; }

    public bool? Accepted { get; set; }
    public string? RejectionMessage { get; set; }
    public bool Blocked { get; set; }
}
