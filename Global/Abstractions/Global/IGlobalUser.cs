using FluentValidation;
using Global.Abstractions.Global.Account;
using Global.Validation;

namespace Global.Abstractions.Global;

public interface IGlobalUser : IIdentityUser
{
    string Forename { get; set; }

    string? MiddleNames { get; set; }

    string Surname { get; set; }

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