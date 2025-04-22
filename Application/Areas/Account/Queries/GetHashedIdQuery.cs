using Application.Areas.Account.BaseModels;
using Application.Shared.Identity;

namespace Application.Areas.Account.Queries;

internal class GetHashedIdQuery : GetHashedIdBaseQuery<HashedUser>
{
    public GetHashedIdQuery(string userNameOrEmail, bool hashed) : base(new CoreIdentityUser())
    {
        IdentityUser.UserName = userNameOrEmail;
        IdentityUser.Email = EmailHelper.IsEmail(userNameOrEmail) ? userNameOrEmail : null;
        IdentityUser.IdentificationHashed = hashed;
    }

    protected async override Task<HashedUser> Handle()
    {
        HashedUser result = await base.Handle();
        return result;
    }
}
