using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Global.Messages;

namespace Application.Areas.Messages.Queries;

public sealed class GetMessagesQuery : BaseQuery<IQueryable<IReadMessage>>
{
    protected override Task<IQueryable<IReadMessage>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);

        var messages = dbCurrentSantaUser.ReceivedMessages
            .AsQueryable()
            .ProjectTo<IReadMessage>(Mapper.ConfigurationProvider);

        return Task.FromResult(messages);
    }
}
