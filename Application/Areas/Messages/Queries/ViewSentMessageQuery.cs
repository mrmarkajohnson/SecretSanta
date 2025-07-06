using Global.Abstractions.Areas.Messages;
using Global.Extensions.Exceptions;

namespace Application.Areas.Messages.Queries;

public sealed class ViewSentMessageQuery : GetMessagesBaseQuery<IReadMessage>
{
    public int MessageKey { get; }

    public ViewSentMessageQuery(int messageKey)
    {
        MessageKey = messageKey;
    }

    protected override Task<IReadMessage> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);
        IQueryable<IReadMessage> sentMessages = GetSentMessages<IReadMessage>(dbCurrentSantaUser);

        IReadMessage? message = sentMessages.FirstOrDefault(x => x.MessageKey == MessageKey);

        if (message == null)
            throw new NotFoundException("Message");

        message.IsSentMessage = true;
        AddPreviousMessages(message, dbCurrentSantaUser);

        return Result(message);
    }
}
