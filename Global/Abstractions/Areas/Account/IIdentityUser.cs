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
            .WithMessage($"Please provide a {UserDisplayNames.UserName} or {UserDisplayNames.Email}.");

        RuleFor(x => x.UserName)
            .MinimumLength(IdentityVal.UserNames.MinLength);

        RuleFor(x => x.Email).EmailAddress();
    }
}