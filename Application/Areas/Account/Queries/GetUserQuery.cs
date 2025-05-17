using Application.Areas.Account.BaseModels;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Account;

namespace Application.Areas.Account.Queries;

public sealed class GetUserQuery : BaseQuery<ISantaUser?>
{
    private readonly string _userNameOrEmail;
    private readonly bool _userNamehashed;
    private readonly string _foreName;
    private readonly bool _unHashResults;

    public GetUserQuery(string userNameOrEmail, bool userNameHashed, string foreName, bool unHashResults)
    {
        _userNameOrEmail = userNameOrEmail;
        _userNamehashed = userNameHashed;
        _foreName = foreName;
        _unHashResults = unHashResults;
    }

    protected async override Task<ISantaUser?> Handle()
    {
        ISantaUser? santaUser = null;

        if (_userNameOrEmail.NotEmpty() && _foreName.NotEmpty())
        {
            HashedUser hashedId = await Send(new GetHashedIdQuery(_userNameOrEmail, _userNamehashed));

            bool isEmail = EmailHelper.IsEmail(_userNameOrEmail);

            var dbGlobalUser = DbContext.Global_Users
                .FirstOrDefault(x => x.Forename.ToLower() == _foreName.ToLower()
                    && (hashedId.UserNameHash != null && x.UserName == hashedId.UserNameHash || isEmail && x.Email == hashedId.EmailHash));

            if (dbGlobalUser != null)
            {
                santaUser = new SantaUser();
                Mapper.Map(dbGlobalUser, santaUser);

                if (_unHashResults)
                {
                    santaUser.UnHash();
                }
            }
        }

        return santaUser;
    }
}
