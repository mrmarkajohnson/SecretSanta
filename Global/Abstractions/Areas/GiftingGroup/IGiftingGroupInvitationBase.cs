using Global.Helpers;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGiftingGroupInvitationBase
{
    Guid InvitationGuid { get; }
    string? InvitationMessage { get; }
}

public static class GiftingGroupInvitationBaseExtensions
{
    public static string GetInvitationId(this IGiftingGroupInvitationBase invitation, string fromUserId)
    {
        return EncryptionHelper.OneWayEncrypt(invitation.InvitationGuid.ToString(), fromUserId, true);
    }
}