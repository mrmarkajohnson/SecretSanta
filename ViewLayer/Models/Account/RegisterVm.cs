using Application.Santa.Areas.Account.BaseModels;
using Global.Abstractions.Global;
using Global.Abstractions.Santa.Areas.Account;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.Account;

public class RegisterVm : SantaUser, IRegisterSantaUser, IForm
{
    public string? ReturnUrl { get; set; }

    //public IList<AuthenticationScheme> ExternalLogins { get; set; }

    [Display(Name = "Password"), DataType(DataType.Password), StringLength(Identity.Passwords.MaxLength, 
        ErrorMessage = "Your {0} must be {2} to {1} characters long.", MinimumLength = Identity.Passwords.MinLength)]
    public required string Password { get; set; }

    [Display(Name = "Confirm password"), DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }

    public string SubmitButtonText { get; set; } = "Register";
    public string SubmitButtonIcon { get; set; } = "fa-id-card";
}
