using Global.Abstractions.Areas.Messages;
using Global.Extensions.Exceptions;

namespace Application.Areas.Messages.Queries;

public sealed class ViewSentMessageQuery : GetMessagesBaseQuery<IReadSentMessage>
{
    public int MessageKey { get; }

    public ViewSentMessageQuery(int messageKey)
    {
        MessageKey = messageKey;
    }

    protected override Task<IReadSentMessage> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);
        IQueryable<IReadSentMessage> sentMessages = GetSentMessages<IReadSentMessage>(dbCurrentSantaUser);

        IReadSentMessage? message = sentMessages.FirstOrDefault(x => x.MessageKey == MessageKey);

        if (message == null)
            throw new NotFoundException("Message");

        message.IsSentMessage = true; // just in case!

        IEnumerable<Santa_Message> allGroupMessages = GetAllGroupMessages(dbCurrentSantaUser);
        AddPreviousMessages(message, dbCurrentSantaUser, allGroupMessages);
        AddLaterMessages(message, dbCurrentSantaUser, allGroupMessages);

        return Result(message);
    }
}
