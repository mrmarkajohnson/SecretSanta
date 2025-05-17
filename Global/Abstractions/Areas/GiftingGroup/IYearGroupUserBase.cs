using Global.Abstractions.Areas.Account;
using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IYearGroupUserBase : IBasicSantaUser, IUserNamesBase
{
    public bool? Included { get; set; }
}
