using Application.Shared.Identity;
using Global.Abstractions.Global.Account;

namespace Application.Santa.Areas.Account.Queries;

public class GetSecurityQuestionsQuery : BaseQuery<ISecurityQuestions?>
{
    private readonly string? _hashedUserName;

    public GetSecurityQuestionsQuery()
    {
    }

    public GetSecurityQuestionsQuery(string userName, bool userNameHashed)
    {
        _hashedUserName = userNameHashed ? userName : EncryptionHelper.TwoWayEncrypt(userName, true);
    }

    protected override Task<ISecurityQuestions?> Handle()
    {
        ISecurityQuestions? securityQuestions = null;
        Global_User? dbGlobalUser = null;

        if (ClaimsUser != null && SignInManager.IsSignedIn(ClaimsUser)) // first constructor
        {
            string? userId = GetCurrentUserId();
            if (userId != null)
            {
                dbGlobalUser = GetGlobalUser(userId);
            }
        }

        if (dbGlobalUser == null && !string.IsNullOrEmpty(_hashedUserName)) // second constructor
        {
            dbGlobalUser = DbContext.Global_Users.FirstOrDefault(x => x.UserName == _hashedUserName);
        }

        if (dbGlobalUser != null)
        {
            securityQuestions = new SecurityQuestions
            {
                SecurityQuestion1 = dbGlobalUser.SecurityQuestion1,
                SecurityAnswer1 = dbGlobalUser.SecurityAnswer1,
                SecurityHint1 = EncryptionHelper.Decrypt(dbGlobalUser.SecurityHint1, false),
                SecurityQuestion2 = dbGlobalUser.SecurityQuestion2,
                SecurityAnswer2 = dbGlobalUser.SecurityAnswer2,
                SecurityHint2 = EncryptionHelper.Decrypt(dbGlobalUser.SecurityHint2, false),
                Greeting = EncryptionHelper.Decrypt(dbGlobalUser.Greeting, false, dbGlobalUser.Id)
            };
        }

        return Task.FromResult(securityQuestions);
    }
}
