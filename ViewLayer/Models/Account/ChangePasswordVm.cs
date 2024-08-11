using FluentValidation;
using Global.Abstractions.Global.Account;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.Account;

public class ChangePasswordVm : SetPasswordBaseVm, IChangePassword
{
    [Required]
    [Display(Name = "Current Password"), DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = "";

    [Required]
    [Display(Name = "New Password"), DataType(DataType.Password)]
    [StringLength(IdentityVal.Passwords.MaxLength, MinimumLength = IdentityVal.Passwords.MinLength)]
    public override string Password { get; set; } = "";
}

public class ChangePasswordVmValidator : SetPasswordValidator<ChangePasswordVm>
{
    public ChangePasswordVmValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotNull().NotEmpty();
    }
}
