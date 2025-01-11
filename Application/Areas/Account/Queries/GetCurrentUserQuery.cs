using Application.Areas.Account.BaseModels;
using Application.Areas.Account.Actions;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Account;
using Global.Extensions.Exceptions;

namespace Application.Areas.Account.Queries;

public class GetCurrentUserQuery : BaseQuery<ISantaUser>
{
    private bool _unHashResults;

    public GetCurrentUserQuery(bool unHashResults)
    {
        _unHashResults = unHashResults;
    }

    protected async override Task<ISantaUser> Handle()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser);

        if (dbCurrentUser.SantaUser != null)
        {
            ISantaUser santaUser = Mapper.Map<SantaUser>(dbCurrentUser);

            if (_unHashResults)
            {
                await Send(new UnHashUserIdentificationAction(santaUser));
            }

            return santaUser;
        }

        throw new AccessDeniedException();
    }
}
