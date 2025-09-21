using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.Shared;

namespace Data.Entities.Santa;

public class Santa_Invitation : ArchivableBaseEntity, IGiftingGroupInvitation
{
    [Key]
    public int InvitationKey { get; set; }
    
    public Guid InvitationGuid { get; set; } = Guid.NewGuid();
    
    public int FromSantaUserKey { get; set; }
    public virtual required Santa_User FromSantaUser { get; set; }

    public int? ToSantaUserKey { get; set; }
    public virtual Santa_User? ToSantaUser { get; set; }

    public int GiftingGroupKey { get; set; }
    public virtual required Santa_GiftingGroup GiftingGroup { get; set; }

    public required string ToName { get; set; }
    public string? ToEmailAddress { get; set; }

    IHashableUser IGiftingGroupInvitation.FromUser => FromSantaUser.GlobalUser;
}
