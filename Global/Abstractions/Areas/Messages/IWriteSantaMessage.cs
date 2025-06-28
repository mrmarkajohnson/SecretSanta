using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Messages;

public interface IWriteSantaMessage : ISendSantaMessage, IChooseMessageRecipient, IHasCalendarYear
{
}
