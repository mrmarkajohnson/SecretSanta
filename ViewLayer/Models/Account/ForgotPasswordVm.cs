using Application.Shared.Identity;
using Global.Abstractions.Global;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.Account;

public class ForgotPasswordVm : SecurityQuestions, IChangePassword, IForm
{
    public string? ReturnUrl { get; set; }

    [Display(Name = "E-mail or Username")]
    public required string EmailOrUserName { get; set; }

    [Display(Name = "First Name")]
    public required string Forename { get; set; }

    [Display(Name = "Password"), DataType(DataType.Password), StringLength(Identity.Passwords.MaxLength,
        ErrorMessage = "Your {0} must be {2} to {1} characters long.", MinimumLength = Identity.Passwords.MinLength)]
    public required string Password { get; set; }

    [Display(Name = "Confirm password"), DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }

    public bool ShowBasicDetails { get; set; }
    public bool ShowSecurityQuestions { get; set; }
    public bool ResetPassword { get; set; }

    public string SubmitButtonText { get; set; } = "Submit";
    public string SubmitButtonIcon { get; set; } = "fa-paper-plane";
}
