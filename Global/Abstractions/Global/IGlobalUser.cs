using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Global;

public interface IGlobalUser : IIdentityUser
{
    [Display(Name = "First Name")]
    string Forename { get; set; }

    [Display(Name = "Middle Names")]
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
            .Length(2, 50);

        RuleFor(x => x.Surname)
            .NotEmpty()
            .Length(2, 50);
    }
}