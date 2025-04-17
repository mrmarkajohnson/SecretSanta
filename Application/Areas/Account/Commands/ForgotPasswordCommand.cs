using Application.Areas.Account.Queries;
using Global.Abstractions.Areas.Account;

namespace Application.Areas.Account.Commands;

public sealed class ForgotPasswordCommand<TItem> : UserBaseCommand<TItem> where TItem : IForgotPassword
{
    public ForgotPasswordCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Item.ResetPassword = false;
        Item.PasswordResetSuccessfully = false;

        if (string.IsNullOrWhiteSpace(Item.EmailOrUserName) || string.IsNullOrWhiteSpace(Item.Forename) || string.IsNullOrWhiteSpace(Item.Greeting))
        {
            SetDetailsNotRecognisedError();
        }
        else
        {
            ISantaUser? user = await Send(new GetUserQuery(Item.EmailOrUserName, false, Item.Forename, false));
            if (user == null || user.UserName == null)
            {
                SetDetailsNotRecognisedError();
            }
            else
            {
                string hashedGreeting = EncryptionHelper.TwoWayEncrypt(Item.Greeting, false, user.GlobalUserId);
                if (user.Greeting != hashedGreeting)
                {
                    SetDetailsNotRecognisedError();
                    Item.LockedOut = await AccessFailed(UserManager, user);
                }
                else
                {
                    ISecurityQuestions? securityQuestions = await Send(new
                        GetSecurityQuestionsQuery(user.UserName, user.IdentificationHashed));

                    if (securityQuestions == null)
                    {
                        SetDetailsNotRecognisedError();
                    }
                    else if (!Item.SecurityQuestionsSet)
                    {
                        SetUpSecurityQuestions(securityQuestions);
                        Success = true;
                    }
                    else
                    {
                        string hashedAnswer1 = EncryptionHelper.OneWayEncrypt(Item.SecurityAnswer1?.ToLower() ?? string.Empty, user);
                        string hashedAnswer2 = EncryptionHelper.OneWayEncrypt(Item.SecurityAnswer2?.ToLower() ?? string.Empty, user);

                        if (hashedAnswer1 != securityQuestions.SecurityAnswer1
                            || hashedAnswer2 != securityQuestions.SecurityAnswer2)
                        {
                            AddGeneralValidationError("Security answers did not match.");
                            SetUpSecurityQuestions(securityQuestions);
                            Item.LockedOut = await AccessFailed(UserManager, user);
                        }
                        else if (string.IsNullOrEmpty(Item.Password) || string.IsNullOrEmpty(Item.ConfirmPassword))
                        {
                            SetUpPasswordReset();
                            Success = true;
                        }
                        else if (Item.ConfirmPassword != Item.Password)
                        {
                            AddGeneralValidationError("Passwords did not match.");
                            SetUpPasswordReset();
                        }
                        else
                        {
                            var commandResult = await Send(new ResetPasswordCommand<TItem>(Item, user), null);

                            if (commandResult.Success)
                            {
                                Item.PasswordResetSuccessfully = true;
                                Success = true;
                            }
                        }
                    }
                }
            }
        }

        return await Result();
    }

    private void SetUpSecurityQuestions(ISecurityQuestions securityQuestions)
    {
        Item.SecurityQuestion1 = securityQuestions.SecurityQuestion1;
        Item.SecurityHint1 = securityQuestions.SecurityHint1;
        Item.SecurityQuestion2 = securityQuestions.SecurityQuestion2;
        Item.SecurityHint2 = securityQuestions.SecurityHint2;
        Item.ShowBasicDetails = false;
        Item.ShowSecurityQuestions = true;
        Item.ResetPassword = false;
        Item.SubmitButtonText = "Send Answers";
        Item.SubmitButtonIcon = "fa-comment-dots";
    }

    private void SetUpPasswordReset()
    {
        Item.ShowBasicDetails = false;
        Item.ShowSecurityQuestions = false;
        Item.ResetPassword = true;
        Item.SubmitButtonText = "Reset";
        Item.SubmitButtonIcon = "fa-rotate-right";
    }

    private void SetDetailsNotRecognisedError()
    {
        AddGeneralValidationError("Details not recognised.");
        Item.ShowBasicDetails = true;
        Item.ShowSecurityQuestions = false;
        Item.ResetPassword = false;
    }
}
