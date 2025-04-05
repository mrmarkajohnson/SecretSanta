namespace ViewLayer.Abstractions;

public interface IModalVm
{
    string ModalTitle { get; }
    bool ShowSaveButton { get; }
}
