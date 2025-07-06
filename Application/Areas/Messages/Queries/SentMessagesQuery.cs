using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.Queries;

public sealed class SentMessagesQuery : GetMessagesBaseQuery<IQueryable<ISantaMessageBase>>
{
    public SentMessagesQuery()
    {
    }

    protected override Task<IQueryable<ISantaMessageBase>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);
        IQueryable<ISantaMessageBase> sentMessages = GetSentMessages<ISantaMessageBase>(dbCurrentSantaUser);
        return Result(sentMessages);
    }
}