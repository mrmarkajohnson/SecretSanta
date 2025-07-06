using Application.Areas.Messages.BaseModels;
using Global.Abstractions.Areas.Messages;
using Global.Abstractions.ViewModels;

namespace Application.Areas.Messages.ViewModels;

public sealed class ReadMessageVm : ReadMessage, IReadMessage, IModalVm
{
    public string ModalTitle => HeaderText;
    public bool ShowSaveButton => false;
    public string? SuccessMessage { get; set; }
    public string? AdditionalFooterButtonPartial => "_ReplyButton";
}
