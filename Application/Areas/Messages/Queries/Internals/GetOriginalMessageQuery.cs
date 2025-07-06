namespace Application.Areas.Messages.Queries.Internals;

internal class GetOriginalMessageQuery : GetMessagesBaseQuery<Santa_Message>
{
    public int MessageKey { get; }
    public Santa_User DbCheckRecipient { get; }

    public GetOriginalMessageQuery(int messageKey, Santa_User dbCheckRecipient)
    {
        MessageKey = messageKey;
        DbCheckRecipient = dbCheckRecipient;
    }

    protected override Task<Santa_Message> Handle()
    {
        return Result(GetOriginalMessage(MessageKey, DbCheckRecipient));
    }
}
