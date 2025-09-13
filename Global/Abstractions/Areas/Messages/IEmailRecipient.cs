using Global.Abstractions.Areas.Account;
using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Messages;

public interface IEmailRecipient : IUserNamesBase, IUserEmailDetails, IIdentityUser
{
    int MessageKey { get; }
    int MessageRecipientKey { get; }
}
