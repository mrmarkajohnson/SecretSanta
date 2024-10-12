using FluentValidation;
using Global.Abstractions.Global.Shared;
using Global.Validation;

namespace Global.Abstractions.Global.Account;

public interface IIdentityUser : IHashableUserId
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