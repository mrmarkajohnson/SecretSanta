using Application.Areas.Account.BaseModels;
using Global.Abstractions.Areas.Account;

namespace Application.Areas.Account.Queries;

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
