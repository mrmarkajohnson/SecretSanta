using Global.Abstractions.Areas.Account;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Models.Account;

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
