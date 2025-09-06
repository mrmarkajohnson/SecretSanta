using Application.Areas.Account.BaseModels;
using Application.Shared.Requests;

namespace Application.Areas.Account.Queries;

public sealed class GetEmailDetailsQuery : BaseQuery<IUserEmailDetails>
{
    public GetEmailDetailsQuery()
    {
    }

    protected override Task<IUserEmailDetails> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser();

        IUserEmailDetails preferences = new EmailDetails
        {
            ReceiveEmails = dbCurrentSantaUser.ReceiveEmails,
            DetailedEmails = dbCurrentSantaUser.DetailedEmails
        };

        return Task.FromResult(preferences);
    }
}
