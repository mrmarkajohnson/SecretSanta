using Application.Shared.ViewModels;
using FluentValidation;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.GiftingGroup.ViewModels;

public class SendGroupInvitationVm : BaseFormVm, ISendGroupInvitation, IFormVm, IModalVm
{
    public SendGroupInvitationVm() : this(new List<IVisibleUser>().AsQueryable(), "")
    {        
    }

    public SendGroupInvitationVm(IQueryable<IVisibleUser> otherGroupMembers, string userGridAction)
    {
        OtherGroupMembers = otherGroupMembers;
        UserGridAction = userGridAction;
        OtherMembersGridModel = new UserGridVm(OtherGroupMembers, UserGridAction);
    }

    public int GiftingGroupKey { get; set; }
    public required string GiftingGroupName { get; set; }

    public Guid InvitationGuid { get; set; }

    [Display(Name = UserDisplayNames.Forename)]
    public string? ToName { get; set; }

    [Display(Name = UserDisplayNames.Email)]
    public string? ToEmailAddress { get; set; }

    [MaxLength(GiftingGroupVal.InvitationMessage.MaxLength)]
    public string Message { get; set; } = string.Empty;

    public string? ToHashedUserId { get; set; }

    public string ModalTitle => "Send a Group Invitation";
    public bool ShowSaveButton => true;
    public string? AdditionalFooterButtonPartial { get; }

    public IQueryable<IVisibleUser> OtherGroupMembers { get; set; }

    public string UserGridAction { get; set; } = ""; // no action needed
    public UserGridVm OtherMembersGridModel { get; set; }

    public override string SubmitButtonText { get; set; } = "Send";
    public override string SubmitButtonIcon { get; set; } = "fa-paper-plane";
}

public class SendGroupInvitationVmValidator : AbstractValidator<SendGroupInvitationVm>
{
    public SendGroupInvitationVmValidator()
    {
        When (x => x.ToHashedUserId.IsNotEmpty(), () => 
        {
            RuleFor(x => x.ToHashedUserId)
                .Must((root, x) => root.OtherGroupMembers.Any(y => y.HashedUserId == x))
                .WithMessage("The user selected is not available.");

            RuleFor(x => x.ToName)
                .Empty()
                .WithMessage(x => x.ToEmailAddress.IsNotEmpty()
                    ? $"You've selected a user and entered a {UserDisplayNames.Forename}. Please choose one or the other."
                    : $"You've selected a user, and also entered a {UserDisplayNames.Forename} and {UserDisplayNames.Email}. Please choose one or the other.");

            RuleFor(x => x.ToEmailAddress)
                .Empty()
                .WithMessage($"You've selected a user and entered an {UserDisplayNames.Email}. Please choose one or the other.");
        })
        .Otherwise(() => 
        {
            RuleFor(x => x.ToName)
                .NotEmpty();

            RuleFor(x => x.ToEmailAddress)
                .NotEmpty();
        });

        RuleFor(x => x.Message).MaximumLength(GiftingGroupVal.InvitationMessage.MaxLength);
    }
}