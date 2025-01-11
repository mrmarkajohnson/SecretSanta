using Global.Abstractions.Global;

namespace Global.Abstractions.Areas.Account;

public interface IBasicSantaUser : IUserNamesBase
{
    int SantaUserId { get; set; }
}
