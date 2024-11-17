namespace Global.Abstractions.Santa.Areas.GiftingGroup;

public interface IGiftingGroupYearBase
{
    int GiftingGroupId { get; set; }
    int Year { get; set; }
    decimal? Limit { get; set; }
}
