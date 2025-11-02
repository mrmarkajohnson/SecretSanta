using Global.Abstractions.Areas.GiftingGroup;
using Global.Helpers;
using Global.Validation;

namespace Data.Entities.Santa;

public class Santa_Invitation : ArchivableBaseEntity, ISendGroupInvitation
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

    public string? ToName { get; set; }

    /// <summary>
    /// This should always be hashed
    /// </summary>
    public string? ToEmailAddress { get; set; }

    [MaxLength(GiftingGroupVal.SendInvitationMessage.MaxLength)]
    public string? InvitationMessage { get; set; }

    [MaxLength(GiftingGroupVal.RejectInvitationMessage.MaxLength)]
    public string? RejectionMessage { get; set; }

    string? ISendGroupInvitation.ToHashedUserId => ToSantaUser?.GlobalUserId;

    public string GetInvitationId()
    {
        return EncryptionHelper.OneWayEncrypt(InvitationGuid.ToString(), FromSantaUser.GlobalUserId, true);
    }
}
