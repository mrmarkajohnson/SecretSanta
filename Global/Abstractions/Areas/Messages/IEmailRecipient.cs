using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Messages;

public interface IEmailRecipient : IUserNamesBase, IUserEmailDetails
{
    int MessageKey { get; }
    int MessageRecipientKey { get; }
}
