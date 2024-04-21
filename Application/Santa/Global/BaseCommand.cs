using FluentValidation;
using FluentValidation.Results;
using Global.Abstractions.Global;
using SecretSanta.Data;

namespace Application.Santa.Global;

public abstract class BaseCommand<TItem>
{
    public TItem Item { get; set; }

    protected bool Success { get; set; }
    protected AbstractValidator<TItem>? Validator { get; set; }
    protected ValidationResult Validation { get; set; } = new ValidationResult();

    protected ApplicationDbContext ModelContext { get; set; }

    protected BaseCommand(TItem item)
    {
        Item = item;
        ModelContext = new ApplicationDbContext();
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
}
