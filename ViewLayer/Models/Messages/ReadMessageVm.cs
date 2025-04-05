using Application.Areas.Messages.BaseModels;
using Global.Abstractions.Global.Messages;
using ViewLayer.Abstractions;

namespace ViewLayer.Models.Messages;

public class ReadMessageVm : ReadMessage, IReadMessage, IModalVm
{
    public string ModalTitle => HeaderText;
    public bool ShowSaveButton => false;
}
