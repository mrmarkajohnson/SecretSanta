using ViewLayer.Abstractions;

namespace ViewLayer.Models.Shared;

public abstract class BasePageVm : IPageVm
{
    public string? SuccessMessage { get; set; }
}
