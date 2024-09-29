namespace Global.Abstractions.Santa.Areas.GiftingGroup;

public interface IGiftingGroupBase
{
    string Name { get; }
    string Description { get; }
    string JoinerToken { get; set; }
}
