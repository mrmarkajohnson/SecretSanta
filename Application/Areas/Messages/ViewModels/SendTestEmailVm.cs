using FluentValidation;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Messages.ViewModels;

public class SendTestEmailVm : IFormVm
{
    [Display(Name = "Recipient Email Address")]
    public string? RecipientEmailAddress { get; set; }
    
    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Submit";
    public string SubmitButtonIcon { get; set; } = "fa-envelope";
    public string? SuccessMessage { get; set; }
}

public class SendTestEmailVmValidator : AbstractValidator<SendTestEmailVm>
{
    public SendTestEmailVmValidator()
    {
        RuleFor(x => x.RecipientEmailAddress).NotEmpty().EmailAddress();
    }
}
