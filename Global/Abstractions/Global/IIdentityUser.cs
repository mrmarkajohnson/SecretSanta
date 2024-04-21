using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Global;

public interface IIdentityUser
{
    string Id { get; set; }

    [Display(Name = "Username")]
    string? UserName { get; set; }

    [Display(Name = "E-mail Address")]
    string? Email { get; set; }

    //[Display(Name = "Phone Number")]
    //string? PhoneNumber { get; set; }
}

public class IdentityUserValidator<T> : AbstractValidator<T> where T : IIdentityUser
{
    public IdentityUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .When(x => string.IsNullOrWhiteSpace(x.Email))
            .WithMessage($"Please provide a Username if no E-mail Address is provided.");
    }
}