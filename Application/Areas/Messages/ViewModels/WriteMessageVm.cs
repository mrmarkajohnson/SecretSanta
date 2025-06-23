using Global.Abstractions.Areas.Messages;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GlobalSettings;

namespace Application.Areas.Messages.ViewModels;

public class WriteMessageVm : ChooseMessageRecipientVm, IWriteSantaMessage, IFormVm, IModalVm
{
    public WriteMessageVm()
    {
        SubmitButtonText = "Send";
        SubmitButtonIcon = "fa-paper-plane";
    }

    [Display(Name = "Title")]
    public string HeaderText { get; set; } = string.Empty;

    [Display(Name = "Message")]
    public string MessageText { get; set; } = string.Empty;

    public bool Important { get; set; }
    public bool CanReply { get; set; }
    public bool ShowAsFromSanta { get; set; }

    public string PageTitle => IsReply ? "Reply to Message" : "Write a Message"; public string ModalTitle => PageTitle;

    public bool IsModal { get; set; }
    public bool ShowSaveButton => true;
    public string GroupWidth => IsModal ? ModalGroupWidth : StandardGroupWidth;
}
