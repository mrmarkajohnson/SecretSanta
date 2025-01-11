namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGiftingGroupBase
{
    string Name { get; }
    string Description { get; set; }
    string JoinerToken { get; set; }
}
