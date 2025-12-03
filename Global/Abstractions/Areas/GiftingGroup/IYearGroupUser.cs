using Global.Abstractions.Areas.Account;
using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IYearGroupUser : IBasicSantaUser, IUserNamesBase
{
    bool? Included { get; }
    int Suggestions { get; }
}
