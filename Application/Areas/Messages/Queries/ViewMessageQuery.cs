using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Global.Messages;
using Global.Extensions.Exceptions;

namespace Application.Areas.Messages.Queries;

public class ViewMessageQuery : BaseQuery<IReadMessage>
{
    public int Id { get; }

    public ViewMessageQuery(int recipientId)
    {
        Id = recipientId;
    }

    protected override Task<IReadMessage> Handle()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.ReceivedMessages);
        if (dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        var message = dbCurrentUser.SantaUser.ReceivedMessages            
            .Where(x => x.Id == Id)
            .AsQueryable()
            .ProjectTo<IReadMessage>(Mapper.ConfigurationProvider)
            .FirstOrDefault();

        if (message == null)
            throw new NotFoundException("Message");

        return Task.FromResult(message);
    }
}
