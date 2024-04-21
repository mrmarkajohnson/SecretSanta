using FluentValidation.Results;
using Global.Abstractions.Global;

namespace Application.Santa.Global;

internal class CommandResult<TItem> : ICommandResult<TItem>
{
    public required TItem Item {  get; set; }
    public bool Success { get; set; }
    public required ValidationResult Validation { get; set; }
}
