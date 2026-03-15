using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Messages;
using Global.Extensions.Exceptions;

namespace Application.Areas.Messages.Queries;

public sealed class ViewMessageQuery : GetMessagesBaseQuery<IReadMessage>
{
    public int MessageKey { get; }
    public int? MessageRecipientKey { get; }

    public ViewMessageQuery(int messageKey, int? messageRecipientKey = null)
    {
        MessageKey = messageKey;
        MessageRecipientKey = messageRecipientKey;
    }

    protected override Task<IReadMessage> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);
        object parameters = new { UserKeysForVisibleEmail = dbCurrentSantaUser.UserKeysForVisibleEmail() };

        IReadMessage? message = null;
        IEnumerable<Santa_Message> allGroupMessages = GetAllGroupMessages(dbCurrentSantaUser);

        var receivedMessages = dbCurrentSantaUser.ReceivedMessages
            .Where(MessageRecipientExpressions.IsActive())
            .Where(x => x.MessageKey == MessageKey);

        if (MessageRecipientKey > 0)
        {
            message = receivedMessages
                .Where(x => x.MessageRecipientKey == MessageRecipientKey)
                .AsQueryable()
                .ProjectTo<IReadMessage>(Mapper.ConfigurationProvider, parameters)
                .FirstOrDefault();
        }

        message ??= receivedMessages
            .AsQueryable()
            .ProjectTo<IReadMessage>(Mapper.ConfigurationProvider, parameters)
            .FirstOrDefault();

        if (message == null)
        {
            message = allGroupMessages
                .Where(x => x.MessageKey == MessageKey)
                .ToList()
                .Where(y => IsIndirectRecipient(dbCurrentSantaUser, y))
                .AsQueryable()
                .ProjectTo<IReadMessage>(Mapper.ConfigurationProvider, parameters)
                .FirstOrDefault();
        }

        if (message == null)
            throw new NotFoundException("Message");

        AddPreviousMessages(message, dbCurrentSantaUser, allGroupMessages);
        AddLaterMessages(message, dbCurrentSantaUser, allGroupMessages);

        return Result(message);
    }
}
