using Global.Abstractions.Global.Account;

namespace Application.Santa.Areas.Account.Commands;

public class SetSecurityQuestionsCommand<TItem> : UserBaseCommand<TItem> where TItem : ISetSecurityQuestions
{
    public SetSecurityQuestionsCommand(TItem item) : base(item)
    {
    }

    protected override async Task<ICommandResult<TItem>> HandlePostValidation()
    {
        EnsureSignedIn();

        string? userId = GetCurrentUserId();
        if (userId != null)
        {
            Global_User? dbGlobalUser = GetGlobalUser(userId);

            if (dbGlobalUser != null)
            {
                if (Item.Update)
                {
                    bool passwordCorrect = await CheckPasswordAndHandleFailure(Item, dbGlobalUser);
                    if (!passwordCorrect || !Validation.IsValid)
                    {
                        return await Result();
                    }
                }
                else if (!Validation.IsValid)
                {
                    return await Result();
                }

                dbGlobalUser.SecurityQuestion1 = Item.SecurityQuestion1;
                dbGlobalUser.SecurityAnswer1 = EncryptionHelper.OneWayEncrypt(Item.SecurityAnswer1?.ToLower() ?? "", dbGlobalUser);
                dbGlobalUser.SecurityHint1 = EncryptionHelper.TwoWayEncrypt(Item.SecurityHint1, false);
                dbGlobalUser.SecurityQuestion2 = Item.SecurityQuestion2;
                dbGlobalUser.SecurityAnswer2 = EncryptionHelper.OneWayEncrypt(Item.SecurityAnswer2?.ToLower() ?? "", dbGlobalUser);
                dbGlobalUser.SecurityHint2 = EncryptionHelper.TwoWayEncrypt(Item.SecurityHint2, false);
                dbGlobalUser.Greeting = EncryptionHelper.TwoWayEncrypt(Item.Greeting, false, dbGlobalUser.Id);

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