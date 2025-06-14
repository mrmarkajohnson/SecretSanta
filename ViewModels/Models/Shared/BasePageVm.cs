using ViewModels.Abstractions;

namespace ViewModels.Models.Shared;

public abstract class BasePageVm : IPageVm
{
    public string? SuccessMessage { get; set; }
}
