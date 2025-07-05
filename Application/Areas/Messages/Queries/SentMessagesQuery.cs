using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Messages;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.Queries;

public sealed class SentMessagesQuery : BaseQuery<IQueryable<ISantaMessageBase>>
{
    public SentMessagesQuery()
    {
    }

    protected override Task<IQueryable<ISantaMessageBase>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);

        IQueryable<ISantaMessageBase> sentMessages = dbCurrentSantaUser.SentMessages
            .Where(x => x.RecipientType != MessageRecipientType.PotentialPartner)
            .AsQueryable()
            .ProjectTo<ISantaMessageBase>(Mapper.ConfigurationProvider);

        return Result(sentMessages);
    }
}