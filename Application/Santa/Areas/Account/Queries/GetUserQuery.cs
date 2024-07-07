using Application.Santa.Areas.Account.Actions;
using Application.Santa.Areas.Account.BaseModels;
using Global.Abstractions.Santa.Areas.Account;

namespace Application.Santa.Areas.Account.Queries;

public class GetUserQuery : BaseQuery<ISantaUser?>
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

    public async override Task<ISantaUser?> Handle()
    {
        ISantaUser? santaUser = null;

        if (!string.IsNullOrWhiteSpace(_userNameOrEmail) && !string.IsNullOrWhiteSpace(_foreName))
        {           
            HashedUserId hashedId = await Send(new GetHashedIdQuery(_userNameOrEmail, _userNamehashed));

            bool isEmail = EmailHelper.IsEmail(_userNameOrEmail);

            var globalUserDb = ModelContext.Global_Users
                .FirstOrDefault(x => x.Forename.ToLower() == _foreName.ToLower()
                    && ((hashedId.UserNameHash != null && x.UserName == hashedId.UserNameHash) || (isEmail && x.Email == hashedId.EmailHash)));
            
            if (globalUserDb != null)
            {
                santaUser = new SantaUser
                {
                    Id = globalUserDb.Id,
                    UserName = globalUserDb.UserName, // note this will be hashed
                    Email = globalUserDb.Email, // note this will be hashed
                    Forename = globalUserDb.Forename,
                    MiddleNames = globalUserDb.MiddleNames,
                    Surname = globalUserDb.Surname,
                    Greeting = globalUserDb.Greeting,
                    SecurityQuestionsSet = globalUserDb.SecurityQuestionsSet,
                    IdentificationHashed = true
                };

                if (_unHashResults)
                {
                    await Send(new UnHashUserIdentificationAction(santaUser));
                }
            }
        }

        return santaUser;
    }
}
