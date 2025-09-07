using Application.Shared.Identity;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Account.BaseModels;

public class UserEmailDetails : HasEmailBase, IUserEmailDetails
{
    public bool EmailConfirmed { get; set; }

    [Display(Name = "Receive E-mails?")]
    public MessageSettings.EmailPreference ReceiveEmails { get; set; }

    [Display(Name = "Receive Full E-mail Text?")]
    public bool DetailedEmails { get; set; }
}

public sealed class EmailDetailsValidator : AbstractValidator<UserEmailDetails>
{
    public EmailDetailsValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}