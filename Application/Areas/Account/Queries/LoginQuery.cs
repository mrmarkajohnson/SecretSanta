using Application.Areas.Account.BaseModels;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Areas.Account.Queries;

public sealed class LoginQuery : BaseQuery<SignInResult>
{
    private readonly ILogin _item;

    public LoginQuery(ILogin item)
    {
        _item = item;
    }

    protected async override Task<SignInResult> Handle()
    {
        HashedUserId hashedId = await Send(new GetHashedIdQuery(_item.EmailOrUserName, false));
        SignInResult result = await SignInManager.PasswordSignInAsync(hashedId.UserNameHash, _item.Password, _item.RememberMe, lockoutOnFailure: true);
        return result;
    }
}
