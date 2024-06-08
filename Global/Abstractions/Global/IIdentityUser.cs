using FluentValidation;
using Global.Validation;

namespace Global.Abstractions.Global;

public interface IIdentityUser
{
    string Id { get; set; }

    string? UserName { get; set; }
    string? Email { get; set; }

    bool IdentificationHashed { get; set; }
}

public class IdentityUserValidator<T> : AbstractValidator<T> where T : IIdentityUser
{
    public IdentityUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .When(x => string.IsNullOrWhiteSpace(x.Email))
            .WithMessage($"Please provide a Username if no E-mail Address is provided.");

        RuleFor(x => x.UserName)
            .MinimumLength(Identity.UserNames.MinLength);

        RuleFor(x => x.Email).EmailAddress();
    }
}