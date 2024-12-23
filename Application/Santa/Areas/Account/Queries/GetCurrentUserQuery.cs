using Application.Santa.Areas.Account.Actions;
using Application.Santa.Areas.Account.BaseModels;
using Global.Abstractions.Santa.Areas.Account;

namespace Application.Santa.Areas.Account.Queries;

public class GetCurrentUserQuery : BaseQuery<ISantaUser?>
{    
    private bool _unHashResults;

    public GetCurrentUserQuery(bool unHashResults)
    {
        _unHashResults = unHashResults;
    }

    protected override async Task<ISantaUser?> Handle()
    {
        ISantaUser? santaUser = null;

        Global_User? dbCurrentUser = GetCurrentGlobalUser();

        if (dbCurrentUser != null)
        {
            santaUser = Mapper.Map<SantaUser>(dbCurrentUser);

            if (_unHashResults)
            {
                await Send(new UnHashUserIdentificationAction(santaUser));
            }
        }

        return santaUser;
    }
}
