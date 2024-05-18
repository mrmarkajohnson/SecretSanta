using Application.Santa.Areas.Account.BaseModels;
using Application.Santa.Global;
using Global.Abstractions.Santa.Areas.Account;

namespace Application.Santa.Areas.Account.Queries;

public class GetUserQuery : BaseQuery<ISantaUser?>
{
    private readonly string _userNameOrEmail;
    private readonly string _foreName;

    public GetUserQuery(string userNameOrEmail, string foreName)
    {
        _userNameOrEmail = userNameOrEmail;
        _foreName = foreName;
    }

    public override Task<ISantaUser?> Handle()
    {
        ISantaUser? santaUser = null;

        if (!string.IsNullOrWhiteSpace(_userNameOrEmail) && !string.IsNullOrWhiteSpace(_foreName))
        {
            bool isEmail = _userNameOrEmail.Contains("@") && _userNameOrEmail.Contains(".");

            var globalUserDb = ModelContext.Global_Users
                .FirstOrDefault(x => x.Forename.ToLower() == _foreName.ToLower()
                    && (x.UserName == _userNameOrEmail || (isEmail && x.Email == _userNameOrEmail)));
            
            if (globalUserDb != null)
            {
                santaUser = new SantaUser
                {
                    Id = globalUserDb.Id,
                    UserName = globalUserDb.UserName,
                    Email = globalUserDb.Email,
                    Forename = globalUserDb.Forename,
                    MiddleNames = globalUserDb.MiddleNames,
                    Surname = globalUserDb.Surname,
                    SecurityQuestionsSet = globalUserDb.SecurityQuestionsSet
                };
            }
        }

        return Task.FromResult(santaUser);
    }
}
