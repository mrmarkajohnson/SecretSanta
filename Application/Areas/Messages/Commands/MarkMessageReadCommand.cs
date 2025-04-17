using Application.Shared.Requests;

namespace Application.Areas.Messages.Commands;

public sealed class MarkMessageReadCommand : BaseCommand<int>
{
    public MarkMessageReadCommand(int messageRecipientKey) : base(messageRecipientKey)
    {
    }

    protected async override Task<ICommandResult<int>> HandlePostValidation()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);

        var message = dbCurrentSantaUser.ReceivedMessages
            .Where(x => x.MessageRecipientKey == Item).FirstOrDefault();

        if (message != null && !message.Read)
        {
            message.Read = true;
            return await SaveAndReturnSuccess();
        }
        else
        {
            return await Result();
        }
    }
}
