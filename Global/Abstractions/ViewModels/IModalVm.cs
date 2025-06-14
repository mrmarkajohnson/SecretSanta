namespace Global.Abstractions.ViewModels;

public interface IModalVm : ISucceedVm
{
    string ModalTitle { get; }
    bool ShowSaveButton { get; }
}
