using FluentValidation.Results;

namespace Application.Shared.Requests;

public sealed class CommandResult<TItem> : ICommandResult<TItem>
{
    public required TItem Item { get; set; }
    public bool Success { get; set; }
    public required ValidationResult Validation { get; set; }
    public string? SuccessMessage { get; set; }
}
