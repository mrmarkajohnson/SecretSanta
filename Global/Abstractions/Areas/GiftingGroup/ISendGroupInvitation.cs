namespace Global.Abstractions.Areas.GiftingGroup;

public interface ISendGroupInvitation : IGiftingGroupInvitationBase
{
    public int GiftingGroupKey { get; }
    public string? ToName { get; }
    public string? ToEmailAddress { get; }
}
