using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Santa.Areas.Account;

public interface IRegisterSantaUser : ISantaUser
{
    [Display(Name = "Password"), DataType(DataType.Password)]
    string Password { get; set; }

    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    string ConfirmPassword { get; set; }
}

public class RegisterSantaValidator : SantaUserValidator<IRegisterSantaUser>
{
    public RegisterSantaValidator()
    {
        RuleFor(x => x.Password).NotEmpty();

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("The password and confirmation password do not match.");
    }
}
