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
    
    public int? ToSantaUserKey { get; set; }

    public string ModalTitle => "Send an Invitation";
    public bool ShowSaveButton => true;
    public string? AdditionalFooterButtonPartial { get; }

    public IQueryable<IVisibleUser> OtherGroupMembers { get; set; }

    public string UserGridAction { get; set; } = ""; // no action needed
    public UserGridVm OtherMembersGridModel { get; set; }
}

public class SendGroupInvitationVmValidator : AbstractValidator<SendGroupInvitationVm>
{
    public SendGroupInvitationVmValidator()
    {
        When (x => x.ToSantaUserKey == null || x.ToSantaUserKey == 0, () => 
        {
            RuleFor(x => x.ToName)
                .NotEmpty();

            RuleFor(x => x.ToEmailAddress)
                .NotEmpty();
        });
    }
}