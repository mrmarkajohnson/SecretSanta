using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Global.Messages;
using Global.Extensions.Exceptions;

namespace Application.Areas.Messages.Queries;

public class GetMessagesQuery : BaseQuery<IQueryable<IReadMessage>>
{
    protected override Task<IQueryable<IReadMessage>> Handle()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.ReceivedMessages);
        if (dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        var messages = dbCurrentUser.SantaUser.ReceivedMessages
            .AsQueryable()
            .ProjectTo<IReadMessage>(Mapper.ConfigurationProvider);

        return Task.FromResult(messages);
    }
}
