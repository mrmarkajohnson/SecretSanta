using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Account;

public interface IBasicSantaUser : IUserNamesBase
{
    int SantaUserKey { get; set; }
}
