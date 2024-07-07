using Application.Santa.Areas.Account.BaseModels;

namespace Application.Santa.Areas.Account.Queries;

public class GetHashedIdWithGreetingQuery : GetHashedIdBaseQuery<HashedUserIdWithGreeting>
{
    public GetHashedIdWithGreetingQuery(IIdentityUser user) : base(user)
    {
    }

    public async override Task<HashedUserIdWithGreeting> Handle()
    {
        HashedUserIdWithGreeting result = await base.Handle();        
        return result;
    }
}
