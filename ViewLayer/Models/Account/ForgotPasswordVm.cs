using FluentValidation;
using Global.Abstractions.Global.Account;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.Account;

public class ForgotPasswordVm : SetPasswordBaseVm, IForgotPassword, IForm
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
    public string Greeting { get; set; } = "";

    [Display(Name = "E-mail or Username")]
    public required string EmailOrUserName { get; set; }

    [Display(Name = "First Name")]
    public required string Forename { get; set; }

    public bool ShowBasicDetails { get; set; }
    public bool ShowSecurityQuestions { get; set; }
    public bool ResetPassword { get; set; }

    public override string SubmitButtonText { get; set; } = "Submit";
    public override string SubmitButtonIcon { get; set; } = "fa-paper-plane";

    public bool SecurityQuestionsSet => !string.IsNullOrWhiteSpace(SecurityAnswer1) && !string.IsNullOrWhiteSpace(SecurityAnswer2);
    public bool PasswordResetSuccessfully { get; set; }
}

public class ForgotPasswordVmValidator : AbstractValidator<ForgotPasswordVm>
{
    public ForgotPasswordVmValidator()
    {
        RuleFor(x => x.EmailOrUserName).NotNull().NotEmpty().When(x => x.ShowBasicDetails);
        RuleFor(x => x.Forename).NotNull().NotEmpty().When(x => x.ShowBasicDetails);

        RuleFor(x => x.SecurityAnswer1).NotNull().NotEmpty().When(x => x.ShowSecurityQuestions);
        RuleFor(x => x.SecurityAnswer2).NotNull().NotEmpty().When(x => x.ShowSecurityQuestions);

        When(x => x.ResetPassword, () =>
        {
            Include(new SetPasswordValidator<ForgotPasswordVm>());
        });        
    }
}
