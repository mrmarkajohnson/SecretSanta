using System.ComponentModel.DataAnnotations;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.ViewModels;

public class WriteReplyVm : WriteMessageVm
{
    public WriteReplyVm()
    {
        GroupKeyPreset = true;
    }
    
    public override bool IsReply => true;

    [Display(Name = "Reply To")]
    public override MessageRecipientType RecipientType { get; set; } = MessageRecipientType.OriginalSender;

    public override string? GroupName { get; set; }

    public override IList<StandardSelectable> GroupSelection => new List<StandardSelectable>();
    public override List<MessageRecipientType> AvailableRecipientTypes => GetReplyRecipientTypes();
    public override IList<StandardSelectable> MemberSelection => new List<StandardSelectable>();
}