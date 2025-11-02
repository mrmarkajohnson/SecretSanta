using Global.Helpers;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGiftingGroupInvitationBase
{
    Guid InvitationGuid { get; }
    string? InvitationMessage { get; }
}