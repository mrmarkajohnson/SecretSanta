using FluentValidation;
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
            .NotEmpty()
            .Length(UserDetails.Forename.MinLength, UserDetails.Forename.MaxLength);

        RuleFor(x => x.Surname)
            .NotEmpty()
            .Length(UserDetails.Surname.MinLength, UserDetails.Surname.MaxLength);
    }
}