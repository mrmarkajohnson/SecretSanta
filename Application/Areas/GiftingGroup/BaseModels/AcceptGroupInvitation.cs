using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.BaseModels;

public class AcceptGroupInvitation : IAcceptGroupInvitation
{
    public Guid InvitationGuid { get; set; }
    public int? ToSantaUserKey { get; set; }
    public required IHashableUser FromUser { get; set; }
}
