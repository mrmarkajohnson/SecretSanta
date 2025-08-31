using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Messages;

public interface IEmailRecipient : IUserNamesBase
{
    int MessageKey { get; }
    int MessageRecipientKey { get; }
}
