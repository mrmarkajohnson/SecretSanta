using FluentValidation;
using FluentValidation.Results;

namespace Application.Santa.Global;

public abstract class BaseCommand<TItem> : BaseRequest
{
    public TItem Item { get; set; }

    protected bool Success { get; set; }
    protected AbstractValidator<TItem>? Validator { get; set; }
    protected ValidationResult Validation { get; set; } = new ValidationResult();

    protected BaseCommand(TItem item)
    {
        Item = item;
    }

    public abstract Task<ICommandResult<TItem>> Handle();

    public async Task<ICommandResult<TItem>> Result()
    {
        var result = new CommandResult<TItem>
        {
            Item = Item,
            Success = Success,
            Validation = Validation
        };

        return await Task.FromResult(result);
    }

    protected async Task<ICommandResult<UItem>> Send<UItem>(BaseCommand<UItem> command)
    {
        ICommandResult<UItem> commandResult = await command.Handle();

        Validation.Errors.AddRange(commandResult.Validation.Errors);

        return commandResult;
    }
}
