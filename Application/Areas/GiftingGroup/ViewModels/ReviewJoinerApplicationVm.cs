using Application.Areas.GiftingGroup.BaseModels;
using FluentValidation;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.GiftingGroup.ViewModels;

public sealed class ReviewJoinerApplicationVm : ReviewJoinerApplication, IFormVm, IReviewApplication
{
    private bool? _accepted;

    /// <summary>
    /// Nullable version of 'Accepted' to ensure the user must choose
    /// </summary>
    [Display(Name = "Allow to Join")]
    public bool? AcceptedOption
    {
        get
        {
            if (Accepted == true) _accepted = true;
            return _accepted;
        }
        set
        {
            _accepted = (Accepted == true && value == null) ? true : value; // don't override Accepted if true
            Accepted = _accepted ?? false;
        }
    }

    public bool SingleApplication { get; set; }

    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Submit";
    public string SubmitButtonIcon { get; set; } = "fa-paper-plane";
    public string? SuccessMessage { get; set; }
}

public sealed class ReviewJoinerApplicationVmValidator : AbstractValidator<ReviewJoinerApplicationVm>
{
    public ReviewJoinerApplicationVmValidator()
    {
        RuleFor(x => x.AcceptedOption).NotNull();
        RuleFor(x => x.RejectionMessage).NotNullOrEmpty().When(x => x.AcceptedOption == false && !x.Blocked);
    }
}
