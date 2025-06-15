using Application.Shared.ViewModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.Areas.Messages;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GlobalSettings;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.ViewModels;

public class SendMessageVm : BaseFormVm, ISendSantaMessage, IFormVm, IModalVm
{
    public SendMessageVm()
    {
        SubmitButtonText = "Send";
        SubmitButtonIcon = "fa-paper-plane";
        GiftingGroups = new List<IUserGiftingGroup>();
    }

    public int? ReplyToMessageKey { get; set; }
    public MessageRecipientType? OriginalRecipientType { get; set; }
    public bool IsReply => ReplyToMessageKey > 0;

    [Display(Name = "Include Future Members")]
    public bool IncludeFutureMembers { get; set; }

    [Display(Name = "To")]
    public MessageRecipientType RecipientType { get; set; } = MessageRecipientType.TBC;

    [Display(Name = "Title")]
    public string HeaderText { get; set; } = string.Empty;

    [Display(Name = "Message")]
    public string MessageText { get; set; } = string.Empty;

    public bool Important { get; set; }
    public bool CanReply { get; set; }
    public bool ShowAsFromSanta { get; set; }

    public int? GiftingGroupKey { get; set; }
    public string? GroupName => GiftingGroupKey > 0 ? GiftingGroups.FirstOrDefault(x => x.GiftingGroupKey == GiftingGroupKey)?.GroupName : null;

    public string PageTitle => IsReply ? "Reply to Message" : "Write Message"; public string ModalTitle => PageTitle;

    public bool IsModal { get; set; }
    public bool ShowSaveButton => true;
    public string GroupWidth => IsModal ? ModalGroupWidth : StandardGroupWidth;

    public IList<IUserGiftingGroup> GiftingGroups { get; set; }
    public IList<StandardSelectable> GroupSelection => GetSelectableGroups();

    public List<MessageRecipientType> AvailableRecipientTypes => IsReply 
        ? ( ReplyRecipientTypes)
        : OriginalRecipientTypes;

    private List<StandardSelectable> GetSelectableGroups()
    {
        return GiftingGroups.Select(x => new StandardSelectable(x.GiftingGroupKey, x.GroupName)).ToList();
    }
}
