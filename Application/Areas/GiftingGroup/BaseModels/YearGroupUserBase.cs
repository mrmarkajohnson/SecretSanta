using Application.Shared.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.BaseModels;

public class YearGroupUserBase : UserNamesBase, IYearGroupUserBase
{
    public int SantaUserKey { get; set; }
    public bool? Included { get; set; }
}
