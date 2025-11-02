using FluentValidation;
using Global.Abstractions.Areas.Account;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Account.ViewModels;

public sealed class RegisterVm : PersonalDetailsBaseVm, IRegisterSantaUser, IFormVm
{
    //public IList<AuthenticationScheme> ExternalLogins { get; set; }

    [Display(Name = "Password"), DataType(DataType.Password)]
    [StringLength(IdentityVal.Passwords.MaxLength, MinimumLength = IdentityVal.Passwords.MinLength)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Confirm Password"), DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = ValidationMessages.PasswordConfirmationError)]
    public string ConfirmPassword { get; set; } = string.Empty;

    public override string SubmitButtonText { get; set; } = "Register";
    public override string SubmitButtonIcon { get; set; } = "fa-id-card";

    public string? InvitationWaitMessage { get; set; }
}

public sealed class RegisterVmValidator : RegisterSantaUserValidator<RegisterVm>
{
    public RegisterVmValidator()
    {
        RuleFor(x => x.SelectGender).NotNull();
    }
}