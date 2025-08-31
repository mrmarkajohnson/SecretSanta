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
        DbContext.EmailClient = new EmailClient(services);

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

    protected async Task<ICommandResult<TItem>> SuccessResult(bool ignoreValidationResult = false)
    {
        Success = Validation.IsValid || ignoreValidationResult;
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

    #region Validation

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

    #endregion Validation

    #region Messages

    protected Santa_Message SendMessage(ISendSantaMessage messageDetails, Santa_User dbSender, Santa_User dbRecipient, Santa_GiftingGroupYear? dbYear = null)
    {
        return SendMessage(messageDetails, dbSender, [dbRecipient], dbYear);
    }

    protected Santa_Message SendMessage(ISendSantaMessage messageDetails, Santa_User dbSender, Santa_User dbRecipient, Santa_GiftingGroup dbGiftingGroup)
    {
        var dbYear = GetOrCreateGiftingGroupYear(dbGiftingGroup);
        return SendMessage(messageDetails, dbSender, [dbRecipient], dbYear);
    }

    protected Santa_Message SendMessage(ISendSantaMessage messageDetails, Santa_User dbSender, IEnumerable<Santa_User> dbRecipients, Santa_GiftingGroupYear? dbYear)
    {
        var dbMessage = new Santa_Message
        {
            Sender = dbSender,
            GiftingGroupYear = dbYear,
            ShowAsFromSanta = messageDetails.ShowAsFromSanta,
            Important = messageDetails.Important,
            HeaderText = messageDetails.HeaderText,
            MessageText = messageDetails.MessageText,
            RecipientType = messageDetails.RecipientType,
            CanReply = messageDetails.CanReply
        };

        foreach (var dbRecipient in dbRecipients)
        {
            dbMessage.Recipients.Add(new Santa_MessageRecipient
            {
                Message = dbMessage,
                RecipientSantaUser = dbRecipient
            });
        }

        DbContext.Santa_Messages.Add(dbMessage);
        return dbMessage;
    }

    protected Santa_Message SendMessage(ISendSantaMessage messageDetails, Santa_User dbSender, IEnumerable<Santa_User> dbRecipients, Santa_GiftingGroup dbGiftingGroup)
    {
        var dbYear = GetOrCreateGiftingGroupYear(dbGiftingGroup);
        return SendMessage(messageDetails, dbSender, dbRecipients, dbYear);
    }

    public string MessageLink(string url, string display, bool addQuotes)
    {
        return DbContext.EmailClient?.MessageLink(url, display, addQuotes) ?? string.Empty;
    }

    #endregion Messages

    #region Gifting Group Years

    protected Santa_GiftingGroupYear GetOrCreateGiftingGroupYear(Santa_GiftingGroup dbGiftingGroup, int year = 0)
    {
        if (year > 0 == false)
            year = DateTime.Today.Year;
        
        Santa_GiftingGroupYear? dbGiftingGroupYear = dbGiftingGroup.Years.FirstOrDefault(x => x.CalendarYear == year);

        if (dbGiftingGroupYear == null)
        {
            dbGiftingGroupYear = CreateGiftingGroupYear(dbGiftingGroup, year);
        }

        return dbGiftingGroupYear;
    }

    protected Santa_GiftingGroupYear CreateGiftingGroupYear(Santa_GiftingGroup dbGiftingGroup, int year)
    {
        Santa_GiftingGroupYear? dbGiftingGroupYear;

        dbGiftingGroupYear = new Santa_GiftingGroupYear
        {
            GiftingGroup = dbGiftingGroup,
            CalendarYear = year,
            CurrencyCode = dbGiftingGroup.GetCurrencyCode(),
            CurrencySymbol = dbGiftingGroup.GetCurrencySymbol()
        };

        dbGiftingGroup.Years.Add(dbGiftingGroupYear);
        DbContext.ChangeTracker.DetectChanges();
        return dbGiftingGroupYear;
    }

    #endregion Gifting Group Years
}
