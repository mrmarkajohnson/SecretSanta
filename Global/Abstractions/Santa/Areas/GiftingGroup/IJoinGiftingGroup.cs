namespace Global.Abstractions.Santa.Areas.GiftingGroup;

public interface IJoinGiftingGroup : IGiftingGroupBase
{
    int? GroupId { get; set; }
    string? Message { get; set; }
    bool Blocked { get; set; }
}
