using Application.Santa.Areas.Account.Actions;
using Application.Santa.Areas.Account.BaseModels;
using Global.Abstractions.Santa.Areas.Account;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.Account.Queries;

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
