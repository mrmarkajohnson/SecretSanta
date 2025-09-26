using Global.Helpers;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGiftingGroupInvitationBase
{
    Guid InvitationGuid { get; }
    int? ToSantaUserKey { get; }
}

public static class GiftingGroupInvitationBasweExtensions
{
    public static string GetInvitationId(this IGiftingGroupInvitationBase invitation, string fromUserId)
    {
        return EncryptionHelper.OneWayEncrypt(invitation.InvitationGuid.ToString(), fromUserId, true);
    }
}