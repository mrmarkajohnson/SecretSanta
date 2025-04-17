using Application.Areas.Account.BaseModels;
using Global.Abstractions.Areas.Account;
using Global.Validation;
using System.ComponentModel.DataAnnotations;
using ViewLayer.Abstractions;

namespace ViewLayer.Models.Account;

public sealed class RegisterVm : SantaUser, IRegisterSantaUser, IFormVm
{
    //public IList<AuthenticationScheme> ExternalLogins { get; set; }

    [Display(Name = "Password"), DataType(DataType.Password), StringLength(IdentityVal.Passwords.MaxLength, MinimumLength = IdentityVal.Passwords.MinLength)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Confirm Password"), DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = ValidationMessages.PasswordConfirmationError)]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    public string? ReturnUrl { get; set; }
    public string? SuccessMessage { get; set; }
    public string SubmitButtonText { get; set; } = "Register";
    public string SubmitButtonIcon { get; set; } = "fa-id-card";
}

public sealed class RegisterVmValidator : RegisterSantaUserValidator<RegisterVm>
{
}