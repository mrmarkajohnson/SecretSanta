using Global.Abstractions.Areas.Account;

namespace Application.Areas.Account.Commands;

public sealed class SetSecurityQuestionsCommand<TItem> : UserBaseCommand<TItem> where TItem : ISetSecurityQuestions
{
    public SetSecurityQuestionsCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        EnsureSignedIn();

        string? globalUserId = GetCurrentUserId();
        if (globalUserId != null)
        {
            Global_User? dbCurrentUser = GetGlobalUser(globalUserId);

            if (dbCurrentUser != null)
            {
                if (Item.Update)
                {
                    bool passwordCorrect = await CheckPasswordAndHandleFailure(Item, dbCurrentUser);
                    if (!passwordCorrect || !Validation.IsValid)
                    {
                        return await Result();
                    }
                }
                else if (!Validation.IsValid)
                {
                    return await Result();
                }

                dbCurrentUser.SecurityQuestion1 = Item.SecurityQuestion1;
                dbCurrentUser.SecurityAnswer1 = EncryptionHelper.OneWayEncrypt(Item.SecurityAnswer1?.ToLower() ?? string.Empty, dbCurrentUser);
                dbCurrentUser.SecurityHint1 = EncryptionHelper.TwoWayEncrypt(Item.SecurityHint1, false);
                dbCurrentUser.SecurityQuestion2 = Item.SecurityQuestion2;
                dbCurrentUser.SecurityAnswer2 = EncryptionHelper.OneWayEncrypt(Item.SecurityAnswer2?.ToLower() ?? string.Empty, dbCurrentUser);
                dbCurrentUser.SecurityHint2 = EncryptionHelper.TwoWayEncrypt(Item.SecurityHint2, false);
                dbCurrentUser.Greeting = EncryptionHelper.TwoWayEncrypt(Item.Greeting, false, dbCurrentUser.Id);

                return await SaveAndReturnSuccess();
            }
            else
            {
                AddUserNotFoundError();
            }
        }
        else
        {
            AddUserNotFoundError();
        }

        return await Result();
    }
}