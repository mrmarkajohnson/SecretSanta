using Application.Areas.Account.BaseModels;
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

        if (_userNameOrEmail.IsNotEmpty() && _foreName.IsNotEmpty())
        {
            HashedUser hashedId = await Send(new GetHashedIdQuery(_userNameOrEmail, _userNamehashed));

            bool isEmail = EmailHelper.IsEmail(_userNameOrEmail);

            var dbPossibleUsers = DbContext.Global_Users
                .Where(x => x.Forename.Trim().ToLower() == _foreName.Trim().ToLower());

            var dbGlobalUser = dbPossibleUsers.FirstOrDefault(x => hashedId.UserNameHash != null && x.UserName == hashedId.UserNameHash)
                ?? dbPossibleUsers.FirstOrDefault(x => isEmail && x.Email != null && x.Email == hashedId.EmailHash);

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
