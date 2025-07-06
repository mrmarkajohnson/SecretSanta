using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.Queries;

public sealed class GetMessagesQuery : GetMessagesBaseQuery<IQueryable<ISantaMessage>>
{
    protected override Task<IQueryable<ISantaMessage>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);

        IQueryable<ISantaMessage> receivedMessages = dbCurrentSantaUser.ReceivedMessages
            .AsQueryable()
            .ProjectTo<ISantaMessage>(Mapper.ConfigurationProvider);

        IQueryable<ISantaMessage> otherAvailableMessages = IndirectMessages(dbCurrentSantaUser)
            .AsQueryable()
            .ProjectTo<ISantaMessage>(Mapper.ConfigurationProvider);

        var messages = receivedMessages.Union(otherAvailableMessages)
            .OrderBy(x => x.Read || !x.Important) // show important unread first
            .ThenBy(x => x.Read) // then all other unread
            .ThenByDescending(x => x.Sent) // then by date sent, descending
            .AsQueryable();

        return Result(messages);
    }
}
