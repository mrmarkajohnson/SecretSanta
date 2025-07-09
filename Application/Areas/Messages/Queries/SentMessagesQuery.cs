using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.Queries;

public sealed class SentMessagesQuery : GetMessagesBaseQuery<IQueryable<ISentMessage>>
{
    public SentMessagesQuery()
    {
    }

    protected override Task<IQueryable<ISentMessage>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);
        IQueryable<ISentMessage> sentMessages = GetSentMessages<ISentMessage>(dbCurrentSantaUser);
        return Result(sentMessages);
    }
}