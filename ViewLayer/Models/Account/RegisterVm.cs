using Application.Santa.Areas.Account.BaseModels;
using Global.Abstractions.Santa.Areas.Account;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.Account;

public class RegisterVm : SantaUser, IRegisterSantaUser, IForm
{
    public string? ReturnUrl { get; set; }
    public string? SuccessMessage { get; set; }

    //public IList<AuthenticationScheme> ExternalLogins { get; set; }

    [Display(Name = "Password"), DataType(DataType.Password), StringLength(IdentityVal.Passwords.MaxLength, MinimumLength = IdentityVal.Passwords.MinLength)]
    public string Password { get; set; } = "";

    [Display(Name = "Confirm Password"), DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = ValidationMessages.PasswordConfirmationError)]
    public string ConfirmPassword { get; set; } = "";

    public string SubmitButtonText { get; set; } = "Register";
    public string SubmitButtonIcon { get; set; } = "fa-id-card";
}

public class RegisterVmValidator : RegisterSantaUserValidator<RegisterVm>
{
}