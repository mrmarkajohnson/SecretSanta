using Application.Areas.Messages.BaseModels;
using Global.Abstractions.Areas.Messages;
using ViewLayer.Abstractions;

namespace ViewLayer.Models.Messages;

public sealed class ReadMessageVm : ReadMessage, IReadMessage, IModalVm
{
    public string ModalTitle => HeaderText;
    public bool ShowSaveButton => false;
    public string? SuccessMessage { get; set; }
}
