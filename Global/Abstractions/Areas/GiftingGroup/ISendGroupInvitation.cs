namespace Global.Abstractions.Areas.GiftingGroup;

public interface ISendGroupInvitation : IGiftingGroupInvitationBase
{
    int GiftingGroupKey { get; }
    string? ToName { get; }
    string? ToEmailAddress { get; }
    string? ToHashedUserId { get; }
}