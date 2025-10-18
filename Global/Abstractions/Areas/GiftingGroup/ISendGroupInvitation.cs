using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface ISendGroupInvitation : IGiftingGroupInvitationBase, IHaveAGroupKey
{
    string? ToName { get; }
    string? ToEmailAddress { get; }
    string? ToHashedUserId { get; }
}