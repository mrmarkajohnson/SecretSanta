using Application.Areas.Account.BaseModels;
using Global.Abstractions.Areas.Account;

namespace Application.Areas.Account.Queries;

internal class GetHashedIdWithGreetingQuery : GetHashedIdBaseQuery<HashedUserWithGreeting>
{
    public GetHashedIdWithGreetingQuery(IIdentityUser identityUser) : base(identityUser)
    {
    }

    protected async override Task<HashedUserWithGreeting> Handle()
    {
        HashedUserWithGreeting result = await base.Handle();
        return result;
    }
}
