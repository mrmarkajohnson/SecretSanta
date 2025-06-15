using FluentValidation;
using Global.Abstractions.Areas.Account;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Account.ViewModels;

public sealed class ForgotPasswordVm : SetPasswordBaseVm, IForgotPassword, IFormVm
{
    public string? SecurityQuestion1 { get; set; }
    public string? SecurityHint1 { get; set; }

    [Required, Display(Name = "Answer 1")]
    public string? SecurityAnswer1 { get; set; }

    public string? SecurityQuestion2 { get; set; }
    public string? SecurityHint2 { get; set; }

    [Required, Display(Name = "Answer 2")]
    public string? SecurityAnswer2 { get; set; }

    [Display(Name = "Greeting")]
    public string Greeting { get; set; } = string.Empty;

    [Display(Name = $"{UserDisplayNames.Email} or {UserDisplayNames.UserName}")]
    public string EmailOrUserName { get; set; } = string.Empty;

    [Display(Name = UserDisplayNames.Forename)]
    public string Forename { get; set; } = string.Empty;

    public bool ShowBasicDetails { get; set; }
    public bool ShowSecurityQuestions { get; set; }
    public bool ResetPassword { get; set; }

    public override string SubmitButtonText { get; set; } = "Submit";
    public override string SubmitButtonIcon { get; set; } = "fa-paper-plane";

    public bool SecurityQuestionsSet => SecurityAnswer1.IsNotEmpty() && SecurityAnswer2.IsNotEmpty();
    public bool PasswordResetSuccessfully { get; set; }
}

public sealed class ForgotPasswordVmValidator : AbstractValidator<ForgotPasswordVm>
{
    public ForgotPasswordVmValidator()
    {
        RuleFor(x => x.EmailOrUserName).NotNullOrEmpty().When(x => x.ShowBasicDetails);
        RuleFor(x => x.Forename).NotNullOrEmpty().When(x => x.ShowBasicDetails);

        RuleFor(x => x.Greeting).IsInDropDownList(x => Greetings.Messages, false).When(x => x.ShowBasicDetails);

        RuleFor(x => x.SecurityAnswer1).NotNull().NotEmpty().When(x => x.ShowSecurityQuestions);
        RuleFor(x => x.SecurityAnswer2).NotNull().NotEmpty().When(x => x.ShowSecurityQuestions);

        When(x => x.ResetPassword, () =>
        {
            Include(new SetPasswordValidator<ForgotPasswordVm>());
        });
    }
}
