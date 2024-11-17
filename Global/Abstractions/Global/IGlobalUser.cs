using FluentValidation;
using Global.Abstractions.Global.Account;
using Global.Abstractions.Global.Shared;
using Global.Validation;

namespace Global.Abstractions.Global;

public interface IGlobalUser : IIdentityUser, IUserAllNames
{
    bool SecurityQuestionsSet { get; }
}

public class GlobalUserValidator<T> : IdentityUserValidator<T> where T : IGlobalUser
{
    public GlobalUserValidator()
    {
        RuleFor(x => x.Forename)
            .NotNull().NotEmpty()
            .Length(UserVal.Forename.MinLength, UserVal.Forename.MaxLength);

        RuleFor(x => x.Surname)
            .NotNull().NotEmpty()
            .Length(UserVal.Surname.MinLength, UserVal.Surname.MaxLength);
    }
}