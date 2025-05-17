using FluentValidation;
using Global.Abstractions.Shared;
using Global.Validation;

namespace Global.Abstractions.Areas.Account;

public interface IIdentityUser : IHashableUser
{
    string Greeting { get; set; }
}

public class IdentityUserValidator<T> : AbstractValidator<T> where T : IIdentityUser
{
    public IdentityUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotNullOrEmpty()
            .When(x => string.IsNullOrWhiteSpace(x.Email))
            .WithMessage($"Please provide a Username if no E-mail Address is provided.");

        RuleFor(x => x.UserName)
            .MinimumLength(IdentityVal.UserNames.MinLength);

        RuleFor(x => x.Email).EmailAddress();
    }
}