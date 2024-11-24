using Global.Abstractions.Global.Shared;
using Global.Abstractions.Santa.Areas.Account;

namespace Global.Abstractions.Santa.Areas.GiftingGroup;

public interface IYearGroupUserBase : IBasicSantaUser, IUserNamesBase
{
    public bool Included { get; set; }
}
