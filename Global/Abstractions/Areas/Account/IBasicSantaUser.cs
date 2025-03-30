using Global.Abstractions.Global;

namespace Global.Abstractions.Areas.Account;

public interface IBasicSantaUser : IUserNamesBase
{
    int SantaUserKey { get; set; }
}
