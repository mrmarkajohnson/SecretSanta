using Application.Santa.Areas.Account.BaseModels;
using Global.Abstractions.Global.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Queries;

public class LoginQuery : BaseQuery<SignInResult>
{
    private readonly ILogin _item;
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginQuery(ILogin item, SignInManager<IdentityUser> signInManager)
    {
        _item = item;
        _signInManager = signInManager;
    }

    protected async override Task<SignInResult> Handle()
    {
        HashedUserId hashedId = await Send(new GetHashedIdQuery(_item.EmailOrUserName, false));
        SignInResult result = await _signInManager.PasswordSignInAsync(hashedId.UserNameHash, _item.Password, _item.RememberMe, lockoutOnFailure: true);
        return result;
    }
}
