namespace Global.Abstractions.Areas.GiftingGroup;

public interface IJoinGiftingGroup : IGiftingGroupBase
{
    int? GiftingGroupId { get; set; }
    string? Message { get; set; }

    bool Blocked { get; set; }
    bool AlreadyMember { get; set; }
    bool ApplicationPending { get; set; }
}
