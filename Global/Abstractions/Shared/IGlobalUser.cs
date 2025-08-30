using FluentValidation;
using Global.Abstractions.Areas.Account;
using Global.Settings;
using Global.Validation;

namespace Global.Abstractions.Shared;

public interface IGlobalUser : IIdentityUser, IUserAllNames
{
    bool SecurityQuestionsSet { get; }
    bool SystemAdmin { get; }
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

        RuleFor(x => x.MiddleNames)
            .NotNullOrEmpty()
            .When(x => x.PreferredNameType == IdentitySettings.PreferredNameOption.MiddleName)
            .WithMessage($"A Middle Name is required when your {UserDisplayNames.PreferredNameType} is your Middle Name");

        RuleFor(x => x.PreferredFirstName)
            .NotNullOrEmpty()
            .When(x => x.PreferredNameType == IdentitySettings.PreferredNameOption.Nickname)
            .WithMessage($"A {UserDisplayNames.PreferredFirstName} is required when your {UserDisplayNames.PreferredNameType} is a Nickname");

        RuleFor(x => x.PreferredFirstName)
            .NotNullOrEmpty()
            .When(x => x.PreferredNameType == IdentitySettings.PreferredNameOption.Other)
            .WithMessage($"A {UserDisplayNames.PreferredFirstName} is required when your {UserDisplayNames.PreferredNameType} type is 'Other'");
    }
}