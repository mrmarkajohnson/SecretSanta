using Application.Shared.ViewModels;
using Global.Abstractions.Areas.Account;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Account.ViewModels;

public abstract class SetPasswordBaseVm : BaseFormVm, ISetPassword, ICheckLockout
{
    [Required]
    [Display(Name = "Password"), DataType(DataType.Password)]
    [StringLength(IdentityVal.Passwords.MaxLength, MinimumLength = IdentityVal.Passwords.MinLength)]
    public virtual string Password { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Confirm Password"), DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = ValidationMessages.PasswordConfirmationError)]
    public string ConfirmPassword { get; set; } = string.Empty;

    public bool LockedOut { get; set; }
}
