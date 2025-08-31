using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.ViewModels;

public class ViewMessagesVm
{
	public ViewMessagesVm(int? messageKey, int? messageRecipientKey)
	{
        MessageKey = messageKey;
        MessageRecipientKey = messageRecipientKey;
    }

    public int? MessageKey { get; }
    public int? MessageRecipientKey { get; }

    public IQueryable<ISantaMessage> Messages { get; set; }
}
