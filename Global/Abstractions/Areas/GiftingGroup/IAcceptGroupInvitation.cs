using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IAcceptGroupInvitation : IGiftingGroupInvitationBase
{    
    IHashableUser FromUser { get; }
    int? ToSantaUserKey { get; }
}

public static class GiftingGroupInvitationExtensions
{
    public static string GetInvitationId(this IAcceptGroupInvitation invitation)
    {
        return invitation.GetInvitationId(invitation.FromUser.GlobalUserId);
    }
}
