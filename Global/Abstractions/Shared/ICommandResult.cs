using FluentValidation.Results;

namespace Global.Abstractions.Global;

public interface ICommandResult<TItem>
{
    TItem Item { get; }
    bool Success { get; }
    ValidationResult Validation { get; }
}
