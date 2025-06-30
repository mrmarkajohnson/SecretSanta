using Application.Areas.Messages.Queries.Internals;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.Queries;

public sealed class GetMessagesQuery : GetMessagesBaseQuery<IQueryable<IReadMessage>>
{
    protected override Task<IQueryable<IReadMessage>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);

        IQueryable<IReadMessage> receivedMessages = dbCurrentSantaUser.ReceivedMessages
            .AsQueryable()
            .ProjectTo<IReadMessage>(Mapper.ConfigurationProvider);

        IQueryable<IReadMessage> otherAvailableMessages = IndirectMessages(dbCurrentSantaUser)
            .AsQueryable()
            .ProjectTo<IReadMessage>(Mapper.ConfigurationProvider);

        var messages = receivedMessages.Union(otherAvailableMessages)
            .OrderBy(x => x.Read || !x.Important) // show important unread first
            .ThenBy(x => x.Read) // then all other unread
            .ThenByDescending(x => x.Sent) // then by date sent, desending
            .AsQueryable();

        return Result(messages);        
    }
}
