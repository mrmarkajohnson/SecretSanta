using Global.Abstractions.Global.Shared;

namespace Global.Abstractions.Santa.Areas.Account;

public interface IBasicSantaUser : IUserAllNames
{
    int SantaUserId { get; set; }
    string UserDisplayName { get; }
}
