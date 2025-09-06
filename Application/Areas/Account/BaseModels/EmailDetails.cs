using Application.Shared.Identity;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Account.BaseModels;

public class EmailDetails : HasEmailBase, IUserEmailDetails
{
    public bool EmailConfirmed { get; }

    [Display(Name = "Receive E-mails?")]
    public MessageSettings.EmailPreference ReceiveEmails { get; set; }

    [Display(Name = "Receive Full E-mail Text?")]
    public bool DetailedEmails { get; set; }
}

public sealed class EmailDetailsValidator : AbstractValidator<EmailDetails>
{
    public EmailDetailsValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}