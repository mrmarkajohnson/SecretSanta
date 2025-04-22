using Application.Areas.Account.BaseModels;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Account;
using Global.Extensions.Exceptions;

namespace Application.Areas.Account.Queries;

public sealed class GetCurrentUserQuery : BaseQuery<ISantaUser>
{
    private bool _unHashResults;

    public GetCurrentUserQuery(bool unHashResults)
    {
        _unHashResults = unHashResults;
    }

    protected override Task<ISantaUser> Handle()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser);

        if (dbCurrentUser.SantaUser != null)
        {
            ISantaUser santaUser = Mapper.Map<SantaUser>(dbCurrentUser);
            santaUser.ShowEmail = true;

            if (_unHashResults)
            {
                santaUser.UnHash();
            }

            return Task.FromResult(santaUser);
        }

        throw new AccessDeniedException();
    }
}
