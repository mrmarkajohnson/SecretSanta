namespace Global.Abstractions.ViewModels;

public interface IOptionalModalFormVm : IFormVm, IModalVm
{
    bool IsModal { get; }
    string PageTitle { get; }
    string? SubTitle { get; }
    List<string> Guidance { get; }
    string GroupWidth { get; }
}
