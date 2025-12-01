using Application.Areas.Account.BaseModels;
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
        HashedUser hashedId = await Send(new GetHashedIdQuery(_item.EmailOrUserName, false));

        bool isEmail = EmailHelper.IsEmail(_item.EmailOrUserName);

        var dbGlobalUser = DbContext.Global_Users.FirstOrDefault(x => hashedId.UserNameHash != null && x.UserName == hashedId.UserNameHash) 
            ?? DbContext.Global_Users.FirstOrDefault(x => isEmail && x.Email != null && x.Email == hashedId.EmailHash);

        SignInResult result = await SignInManager.PasswordSignInAsync(dbGlobalUser?.NormalizedUserName ?? hashedId.UserNameHash, 
            _item.Password, _item.RememberMe, lockoutOnFailure: true);

        return result;
    }
}
