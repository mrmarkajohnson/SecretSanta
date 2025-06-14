using Application.Areas.Messages.BaseModels;
using Global.Abstractions.Areas.Messages;
using ViewModels.Abstractions;

namespace ViewModels.Models.Messages;

public sealed class ReadMessageVm : ReadMessage, IReadMessage, IModalVm
{
    public string ModalTitle => HeaderText;
    public bool ShowSaveButton => false;
    public string? SuccessMessage { get; set; }
}
