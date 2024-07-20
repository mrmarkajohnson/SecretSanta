using Global.Abstractions.Global.Account;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.Account;

public abstract class SetPasswordBaseVm : BaseFormVm, ISetPassword, ICheckLockout
{
    [Required]
    [Display(Name = "Password"), DataType(DataType.Password)] 
    [StringLength(IdentityVal.Passwords.MaxLength, MinimumLength = IdentityVal.Passwords.MinLength)]
    public virtual required string Password { get; set; }

    [Required]
    [Display(Name = "Confirm Password"), DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = ValidationMessages.PasswordConfirmationError)]
    public required string ConfirmPassword { get; set; }

    public bool LockedOut { get; set; }
}
