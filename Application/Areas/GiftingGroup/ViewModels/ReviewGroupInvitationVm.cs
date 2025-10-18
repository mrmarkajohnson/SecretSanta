using Application.Areas.GiftingGroup.BaseModels;
using FluentValidation;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.ViewModels;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Areas.GiftingGroup.ViewModels;

public class ReviewGroupInvitationVm : ReviewGroupInvitation, IReviewGroupInvitation, IFormVm, IGroupMembersGridVm
{
    public OtherGroupMembersType MemberListType => OtherGroupMembersType.ReviewInvitation;

    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";
    public string? SuccessMessage { get; set; }

    public IEnumerable<IGroupMember> OtherGroupMembers { get; set; } = new List<IGroupMember>();

    Guid? IGroupMembersGridVm.InvitationGuid => InvitationGuid;
}

public class ReviewGroupInvitationVmValidator : AbstractValidator<ReviewGroupInvitationVm>
{
    public ReviewGroupInvitationVmValidator()
    {
        RuleFor(x => x.Accept)
            .NotNull()
            .WithMessage("Please choose whether to accept or reject the invitation, or select 'Not sure' to return to it later.");
        
        RuleFor(x => x.RejectionMessage)
            .NotEmpty()
            .When(x => x.Accept == GlobalSettings.YesNoNotSure.No)
            .WithMessage("Please explain why you're rejecting this invitation.");

        RuleFor(x => x.RejectionMessage).MaximumLength(GiftingGroupVal.RejectInvitationMessage.MaxLength);
    }
}