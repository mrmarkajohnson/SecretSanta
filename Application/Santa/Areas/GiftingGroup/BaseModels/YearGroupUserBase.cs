using Global.Abstractions.Santa.Areas.GiftingGroup;

namespace Application.Santa.Areas.GiftingGroup.BaseModels;

public class YearGroupUserBase : UserNamesBase, IYearGroupUserBase
{
    public int SantaUserId { get; set; }
    public bool Included { get; set; }
}
