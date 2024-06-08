using FluentValidation.Results;

namespace Application.Santa.Global;

public class CommandResult<TItem> : ICommandResult<TItem>
{
    public required TItem Item {  get; set; }
    public bool Success { get; set; }
    public required ValidationResult Validation { get; set; }
}
