using Global.Abstractions.Areas.Account;
using Global.Abstractions.Shared;
using Global.Helpers;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGiftingGroupInvitation
{
    Guid InvitationGuid { get; }
    int? ToSantaUserKey { get; }
    IHashableUser FromUser { get; }
}

public static class GiftingGroupInvitationExtensions
{
    public static string GetInvitationId(this IGiftingGroupInvitation invitation)
    {
        return EncryptionHelper.OneWayEncrypt(invitation.InvitationGuid.ToString(), invitation.FromUser, true);
    }
}
