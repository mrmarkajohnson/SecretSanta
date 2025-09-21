using FluentValidation.Results;

namespace Global.Abstractions.Shared;

public interface ICommandResult<TItem>
{
    TItem Item { get; }
    bool Success { get; }
    ValidationResult Validation { get; }
    string? SuccessMessage { get; set; }
}
