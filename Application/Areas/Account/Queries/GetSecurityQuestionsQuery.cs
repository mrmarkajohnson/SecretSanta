using Application.Shared.Identity;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Account;

namespace Application.Areas.Account.Queries;

public sealed class GetSecurityQuestionsQuery : BaseQuery<ISecurityQuestions?>
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
        Global_User? dbCurrentUser = null;

        if (ClaimsUser != null && SignInManager.IsSignedIn(ClaimsUser)) // first constructor
        {
            string? globalUserId = GetCurrentUserId();
            if (globalUserId != null)
            {
                dbCurrentUser = GetGlobalUser(globalUserId);
            }
        }

        if (dbCurrentUser == null && !string.IsNullOrEmpty(_hashedUserName)) // second constructor
        {
            dbCurrentUser = DbContext.Global_Users.FirstOrDefault(x => x.UserName == _hashedUserName);
        }

        if (dbCurrentUser != null)
        {
            securityQuestions = new SecurityQuestions
            {
                SecurityQuestion1 = dbCurrentUser.SecurityQuestion1,
                SecurityAnswer1 = dbCurrentUser.SecurityAnswer1,
                SecurityHint1 = EncryptionHelper.Decrypt(dbCurrentUser.SecurityHint1, false),
                SecurityQuestion2 = dbCurrentUser.SecurityQuestion2,
                SecurityAnswer2 = dbCurrentUser.SecurityAnswer2,
                SecurityHint2 = EncryptionHelper.Decrypt(dbCurrentUser.SecurityHint2, false),
                Greeting = EncryptionHelper.Decrypt(dbCurrentUser.Greeting, false, dbCurrentUser.Id)
            };
        }

        return Task.FromResult(securityQuestions);
    }
}
