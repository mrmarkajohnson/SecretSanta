using Application.Shared.ViewModels;
using FluentValidation;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.ViewModels;

namespace Application.Areas.GiftingGroup.ViewModels;

public class SendGroupInvitationVm : BaseFormVm, ISendGroupInvitation, IFormVm, IModalVm
{
    public int GiftingGroupKey { get; set; }
    public required string GiftingGroupName { get; set; }

    public Guid InvitationGuid { get; set; }

    public string? ToName { get; set; }
    public string? ToEmailAddress { get; set; }    
    public int? ToSantaUserKey { get; set; }

    public string ModalTitle => "Send an Invitation";
    public bool ShowSaveButton => true;
    public string? AdditionalFooterButtonPartial { get; }

    public IQueryable<IVisibleUser> OtherGroupMembers { get; set; } = new List<IVisibleUser>().AsQueryable();
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