using Global.Abstractions.Global.Shared;

namespace Global.Abstractions.Santa.Areas.Account;

public interface IBasicSantaUser : IUserNamesBase
{
    int SantaUserId { get; set; }
}
