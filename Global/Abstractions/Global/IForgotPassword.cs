namespace Global.Abstractions.Global;

public interface IForgotPassword : ISecurityQuestions, IChangePassword, IForm
{
    string Forename { get; set; }

    bool ShowBasicDetails { get; set; }
    bool ShowSecurityQuestions { get; set; }
    bool ResetPassword { get; set; }
    bool PasswordResetSuccessfully { get; set; }
}
