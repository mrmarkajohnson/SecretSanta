using FluentValidation;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Global.Account;

public interface ISetPassword
{
    [DataType(DataType.Password)]
    string Password { get; set; }

    [Display(Name = "Confirm Password"), DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = ValidationMessages.PasswordConfirmationError)]
    string ConfirmPassword { get; set; }
}

public class SetPasswordValidator<T> : AbstractValidator<T> where T : ISetPassword
{
    public SetPasswordValidator()
    {
        RuleFor(x => x.Password)
            .NotNull().NotEmpty()
            .Length(IdentityVal.Passwords.MinLength, IdentityVal.Passwords.MaxLength);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .When(x => !string.IsNullOrWhiteSpace(x.Password))
            .WithMessage(ValidationMessages.PasswordConfirmationError);
    }
}
