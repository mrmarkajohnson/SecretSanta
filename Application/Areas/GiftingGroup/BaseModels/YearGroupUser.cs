using Application.Shared.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.BaseModels;

public sealed class YearGroupUser : UserNamesBase, IYearGroupUser
{
    public int SantaUserKey { get; set; }
    public bool? Included { get; set; }
    public int Suggestions { get; set; }
}
