using Application.Areas.Messages.Queries.Internal;
using Application.Shared.Requests;

namespace Application.Areas.Messages.Commands;

public sealed class MarkMessageReadCommand : BaseCommand<int>
{
    public int MessageKey { get; }
    public int? MessageRecipientKey { get; }

    public MarkMessageReadCommand(int messageKey, int? messageRecipientKey) : base(messageKey)
    {
        MessageKey = messageKey;
        MessageRecipientKey = messageRecipientKey;
    }

    protected async override Task<ICommandResult<int>> HandlePostValidation()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);

        Santa_MessageRecipient? dbRecipient = null;

        var receivedMessages = dbCurrentSantaUser.ReceivedMessages
            .Where(x => x.DateArchived == null)
            .Where(x => x.Message.DateArchived == null)
            .Where(x => x.MessageKey == MessageKey);

        if (MessageRecipientKey > 0)
        {
            dbRecipient = receivedMessages
                .Where(x => x.MessageRecipientKey == MessageRecipientKey)
                .FirstOrDefault();
        }

        dbRecipient ??= receivedMessages.FirstOrDefault();

        if (dbRecipient == null)
        {
            var indirectMessages = await Send(new GetIndirectMessagesQuery(dbCurrentSantaUser));
            var dbMessage = indirectMessages.FirstOrDefault(x => x.MessageKey == MessageKey);

            if (dbMessage != null)
            {
                return await CreateRecipientAsRead(dbCurrentSantaUser, dbMessage);
            }
        }
        else if (!dbRecipient.Read)
        {
            dbRecipient.Read = true;
            return await SaveAndReturnSuccess();
        }

        Success = true; // always treat as successful, to avoid unnecessary problems
        return await Result();
    }

    private async Task<ICommandResult<int>> CreateRecipientAsRead(Santa_User dbCurrentSantaUser, Santa_Message dbMessage)
    {
        var dbRecipient = new Santa_MessageRecipient
        {
            MessageKey = MessageKey,
            Message = dbMessage,
            RecipientSantaUserKey = dbCurrentSantaUser.SantaUserKey,
            RecipientSantaUser = dbCurrentSantaUser,
            Read = true
        };

        dbMessage.Recipients.Add(dbRecipient);
        return await SaveAndReturnSuccess();
    }
}
