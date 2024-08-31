using Application.Santa.Areas.Account.BaseModels;
using Global.Abstractions.Global.Account;

namespace Application.Santa.Areas.Account.Queries;

internal class GetHashedIdWithGreetingQuery : GetHashedIdBaseQuery<HashedUserIdWithGreeting>
{
    public GetHashedIdWithGreetingQuery(IIdentityUser identityUser) : base(identityUser)
    {
    }

    protected async override Task<HashedUserIdWithGreeting> Handle()
    {
        HashedUserIdWithGreeting result = await base.Handle();        
        return result;
    }
}
