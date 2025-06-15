using FluentValidation;
using FluentValidation.Results;
using Global.Abstractions.Areas.Messages;
using System.Security.Claims;

namespace Application.Shared.Requests;

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

    public async override Task<ICommandResult<TItem>> Handle(IServiceProvider services, ClaimsPrincipal claimsUser)
    {
        Initialise(services, claimsUser);

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

    /// <summary>
    /// Save changes on the main context, set success and return a success command result
    /// If using this to save new entities, you may need to update the Item with the new keys before returning
    /// </summary>
    protected async Task<ICommandResult<TItem>> SaveAndReturnSuccess(bool ignoreValidationResult = false)
    {
        if (Validation.IsValid || ignoreValidationResult)
        {
            await DbContext.SaveChangesAsync();
            Success = true;
        }
        else
        {
            Success = false;
        }

        return await Result();
    }

    protected async Task<ICommandResult<TItem>> Result()
    {
        var result = new CommandResult<TItem>
        {
            Item = Item,
            Success = Success,
            Validation = Validation
        };

        return await Task.FromResult(result);
    }

    protected async Task<ICommandResult<TExtraItem>> Send<TExtraItem>(BaseCommand<TExtraItem> subCommand, AbstractValidator<TExtraItem>? validator)
    {
        if (validator != null)
        {
            subCommand.Validator = validator;
        }

        if (Services == null)
        {
            throw new ArgumentException("Services cannot be null");
        }

        ICommandResult<TExtraItem> commandResult = await subCommand.Handle(Services, ClaimsUser);

        Validation.Errors.AddRange(commandResult.Validation.Errors);

        return commandResult;
    }

    protected void AddUserNotFoundError()
    {
        AddGeneralValidationError("User not found. Please log in again.");
    }

    protected void AddGeneralValidationError(string errorMessage)
    {
        AddValidationError(string.Empty, errorMessage);
    }

    protected void AddValidationError(string propertyName, string errorMessage)
    {
        Validation.Errors.Add(new ValidationFailure(propertyName, errorMessage));
    }

    protected void SendMessage(ISendSantaMessage messageDetails, Santa_User dbSender, Santa_User dbRecipientSantaUser, Santa_GiftingGroupYear? dbYear = null)
    {
        var dbMessage = new Santa_Message
        {
            Sender = dbSender,
            GiftingGroupYear = dbYear,
            ShowAsFromSanta = messageDetails.ShowAsFromSanta,
            Important = messageDetails.Important,
            HeaderText = messageDetails.HeaderText,
            MessageText = messageDetails.MessageText,
            RecipientType = messageDetails.RecipientType
        };

        dbMessage.Recipients.Add(new Santa_MessageRecipient
        {
            Message = dbMessage,
            RecipientSantaUser = dbRecipientSantaUser
        });

        DbContext.Santa_Messages.Add(dbMessage);
    }
}
