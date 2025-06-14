using Global.Abstractions.ViewModels;

namespace Application.Shared.ViewModels;

public abstract class BasePageVm : IPageVm
{
    public string? SuccessMessage { get; set; }
}
