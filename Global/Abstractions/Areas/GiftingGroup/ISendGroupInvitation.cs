using Global.Abstractions.Shared;
using Global.Helpers;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface ISendGroupInvitation : IGiftingGroupInvitationBase, IHaveAGroupKey
{
    string? ToName { get; }

    /// <summary>
    /// This may be hashed (always if it's for the entity)
    /// </summary>
    string? ToEmailAddress { get; }

    string? ToHashedUserId { get; }
}

public static class SendGroupInvitationExtensions
{
    public static string GetHashedEmail(this ISendGroupInvitation invitation)
    {
        return EncryptionHelper.EncryptEmail(invitation.ToEmailAddress.Tidy(false));
    }
}