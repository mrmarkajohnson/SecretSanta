namespace ViewLayer.Abstractions;

public interface IModalVm : ISucceedVm
{
    string ModalTitle { get; }
    bool ShowSaveButton { get; }
}
