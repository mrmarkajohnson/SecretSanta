using FluentValidation;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Global;

public interface ISetPassword
{
    [DataType(DataType.Password)]
    string Password { get; set; }

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
