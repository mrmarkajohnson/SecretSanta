using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Global.Messages;
using Global.Extensions.Exceptions;

namespace Application.Areas.Messages.Queries;

public class ViewMessageQuery : BaseQuery<IReadMessage>
{
    public int MessageRecipientKey { get; }

    public ViewMessageQuery(int messageRecipientKey)
    {
        MessageRecipientKey = messageRecipientKey;
    }

    protected override Task<IReadMessage> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.ReceivedMessages);

        var message = dbCurrentSantaUser.ReceivedMessages            
            .Where(x => x.MessageRecipientKey == MessageRecipientKey)
            .AsQueryable()
            .ProjectTo<IReadMessage>(Mapper.ConfigurationProvider)
            .FirstOrDefault();

        if (message == null)
            throw new NotFoundException("Message");

        return Task.FromResult(message);
    }
}
