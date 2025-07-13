namespace Application.Areas.Messages.Queries.Internals;

internal class GetOriginalMessageQuery : GetMessagesBaseQuery<Santa_Message>
{
    public int MessageKey { get; }
    public Santa_User DbSantaUser { get; }
    public bool GetFirstMessageIfSent { get; }

    public GetOriginalMessageQuery(int messageKey, Santa_User dbSantaUser, bool getFirstMessageIfSent)
    {
        MessageKey = messageKey;
        DbSantaUser = dbSantaUser;
        GetFirstMessageIfSent = getFirstMessageIfSent;
    }

    protected override Task<Santa_Message> Handle()
    {
        return Result(GetOriginalMessage(MessageKey, DbSantaUser, GetFirstMessageIfSent, true));
    }
}
