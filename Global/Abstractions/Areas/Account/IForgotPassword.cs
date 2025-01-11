using Global.Abstractions.Global;

namespace Global.Abstractions.Areas.Account;

public interface IForgotPassword : ISecurityQuestions, IResetPassword, IForm
{
    string Forename { get; set; }

    bool ShowBasicDetails { get; set; }
    bool ShowSecurityQuestions { get; set; }
    bool ResetPassword { get; set; }
    bool PasswordResetSuccessfully { get; set; }
}
