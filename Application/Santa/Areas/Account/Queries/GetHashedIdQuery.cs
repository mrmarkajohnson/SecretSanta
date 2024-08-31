using Application.Santa.Areas.Account.BaseModels;
using Application.Shared.Identity;

namespace Application.Santa.Areas.Account.Queries;

internal class GetHashedIdQuery : GetHashedIdBaseQuery<HashedUserId>
{
    public GetHashedIdQuery(string userNameOrEmail, bool hashed) : base(new CoreIdentityUser())
    {
        IdentityUser.UserName = userNameOrEmail;
        IdentityUser.Email = EmailHelper.IsEmail(userNameOrEmail) ? userNameOrEmail : null;
        IdentityUser.IdentificationHashed = hashed;
    }

    protected async override Task<HashedUserId> Handle()
    {
        HashedUserId result = await base.Handle();
        return result;
    }
}
