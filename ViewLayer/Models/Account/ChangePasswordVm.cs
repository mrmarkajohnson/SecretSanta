using FluentValidation;
using Global.Abstractions.Areas.Account;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.Account;

public sealed class ChangePasswordVm : SetPasswordBaseVm, IChangePassword
{
    [Required]
    [Display(Name = "Current Password"), DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [Display(Name = "New Password"), DataType(DataType.Password)]
    [StringLength(IdentityVal.Passwords.MaxLength, MinimumLength = IdentityVal.Passwords.MinLength)]
    public override string Password { get; set; } = string.Empty;
}

public sealed class ChangePasswordVmValidator : SetPasswordValidator<ChangePasswordVm>
{
    public ChangePasswordVmValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotNull().NotEmpty();
    }
}
