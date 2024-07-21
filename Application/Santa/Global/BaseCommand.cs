using FluentValidation;
using FluentValidation.Results;

namespace Application.Santa.Global;

public abstract class BaseCommand<TItem> : BaseRequest<ICommandResult<TItem>>
{
    public TItem Item { get; set; }
    public AbstractValidator<TItem>? Validator { get; set; }
    public ValidationResult Validation { get; set; } = new ValidationResult();

    protected bool Success { get; set; }

    protected BaseCommand(TItem item)
    {
        Item = item;
    }

    public override async Task<ICommandResult<TItem>> Handle(IServiceProvider services)
    {
        Initialise(services);

        if (Validator != null && !Validation.RuleSetsExecuted.Any())
        {
            Validator.Validate(Item);
        }

        if (Validation.IsValid)
        {
            return await HandlePostValidation();
        }
        else
        {
            Success = false; // just in case
            return await Result();
        }
    }

    protected abstract Task<ICommandResult<TItem>> HandlePostValidation();

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

    protected async Task<ICommandResult<UItem>> Send<UItem>(BaseCommand<UItem> subCommand, AbstractValidator<UItem>? validator)
    {
        if (validator != null)
        {
            subCommand.Validator = validator;
        }

        if (Services == null)
        {
            throw new ArgumentException("Services cannot be null");
        }

        ICommandResult<UItem> commandResult = await subCommand.Handle(Services);

        Validation.Errors.AddRange(commandResult.Validation.Errors);

        return commandResult;
    }

    protected void AddUserNotFoundError()
    {
        AddValidationError(string.Empty, "User not found. Please log in again.");
    }

    protected void AddValidationError(string propertyName, string errorMessage)
    {
        Validation.Errors.Add(new ValidationFailure(propertyName, errorMessage));
    }
}
