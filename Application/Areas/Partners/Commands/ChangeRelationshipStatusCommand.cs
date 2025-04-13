using Application.Areas.Messages.BaseModels;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Partners;
using Global.Extensions.Exceptions;
using static Global.Settings.MessageSettings;
using static Global.Settings.PartnerSettings;

namespace Application.Areas.Partners.Commands;

public class ChangeRelationshipStatusCommand : BaseCommand<IChangeRelationshipStatus>
{
    public ChangeRelationshipStatusCommand(IChangeRelationshipStatus item) : base(item)
    {
    }

    private Global_User? _dbCurrentUser;
    private List<Santa_PartnerLink> _dbPossibleRelationships = new();
    private bool _currentUserSuggested;
    private string _headerText = "";
    private string _messageText = "";

    protected async override Task<ICommandResult<IChangeRelationshipStatus>> HandlePostValidation()
    {
        _dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser,
            g => g.SantaUser.SuggestedRelationships, g => g.SantaUser.ConfirmingRelationships);

        if (_dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        _dbPossibleRelationships = _dbCurrentUser.SantaUser.SuggestedRelationships
            .Where(x => x.DateArchived == null && x.DateDeleted == null
                && x.ConfirmingSantaUser.GlobalUserId == Item.GlobalUserId.ToString())
            .Union(_dbCurrentUser.SantaUser.ConfirmingRelationships
                .Where(x => x.DateArchived == null && x.DateDeleted == null
                    && x.SuggestedBySantaUser.GlobalUserId == Item.GlobalUserId.ToString()))
            .ToList();

        Santa_PartnerLink? dbRelationship = _dbPossibleRelationships.FirstOrDefault(x => x.PartnerLinkKey == Item.PartnerLinkKey);

        if (dbRelationship == null)
        {
            AddGeneralValidationError("No matching relationship found. Please reload the page.");
            return await Result();
        }

        _currentUserSuggested = dbRelationship.SuggestedBySantaUserKey == _dbCurrentUser.SantaUser.SantaUserKey;

        HandleStatusChange(dbRelationship);

        if (!string.IsNullOrWhiteSpace(_messageText))
        {
            SendUpdateMessage(dbRelationship);
        }

        return await SaveAndReturnSuccess();
    }

    private void HandleStatusChange(Santa_PartnerLink dbRelationship)
    {
        dbRelationship.ExchangeGifts = false; // as a starting position

        switch (Item.NewStatus)
        {
            case RelationshipStatus.ToBeConfirmed:
            case RelationshipStatus.ToConfirm:
                AddInvalidStatusError();
                break;
            case RelationshipStatus.Active:
                ConfirmRelationship(dbRelationship);
                break;
            case RelationshipStatus.Ended:
                EndConfirmedRelationship(dbRelationship);
                break;
            case RelationshipStatus.EndedBeforeConfirmation:
                EndBeforeConfirmation(dbRelationship);
                break;
            case RelationshipStatus.IgnoreOld:
                IgnoreOldRelationship(dbRelationship);
                break;
            case RelationshipStatus.IgnoreNonRelationship:
                IgnoreNonRelationship(dbRelationship);
                break;
            case RelationshipStatus.Avoid:
                AvoidRelationship(dbRelationship);
                break;
            default: throw new NotImplementedException();
        }
    }

    private void ConfirmRelationship(Santa_PartnerLink dbRelationship)
    {
        if (dbRelationship.RelationshipEnded != null) // just in case
        {
            AddGeneralValidationError("You cannot reactivate an old relationship, as your partner needs to confirm that you are now in a relationship again. " +
                "Please mark this one as ended, then create another.");
            return;
        }

        if (_currentUserSuggested && dbRelationship.Confirmed != true)
        {
            AddInvalidStatusError();
            return;
        }

        dbRelationship.Confirmed = true;
        ArchiveAnyOtherRelationships();

        _headerText = "Your relationship is confirmed";
        _messageText = $"{_dbCurrentUser?.FullName()} confirmed that they're in a relationship with you. " +
            $"Congratulations!";
    }

    private void EndConfirmedRelationship(Santa_PartnerLink dbRelationship)
    {
        if (_currentUserSuggested && dbRelationship.Confirmed != true)
        {
            AddInvalidStatusError();
            return;
        }

        if (dbRelationship.RelationshipEnded == null)
        {
            _headerText = "Sorry to hear your relationship has ended";
            _messageText = $"Santa is sorry to hear that {_dbCurrentUser?.FullName()} said that they're no " +
                $"longer in a relationship with you. Santa hopes that you're both OK.";
        }

        if (_currentUserSuggested) // may have changed their mind about exchanging gifts 
        {
            dbRelationship.SuggestedByIgnoreOld = false;
        }
        else
        {
            dbRelationship.Confirmed = true;
            dbRelationship.ConfirmingUserIgnore = false;
        }

        dbRelationship.ExchangeGifts = false; // just in case
        dbRelationship.RelationshipEnded ??= DateTime.Now;
    }

    private void EndBeforeConfirmation(Santa_PartnerLink dbRelationship)
    {
        if (!_currentUserSuggested)
        {
            AddInvalidStatusError();
            return;
        }

        dbRelationship.ExchangeGifts = false; // just in case
        dbRelationship.RelationshipEnded ??= DateTime.Now;
        dbRelationship.SuggestedByIgnoreOld = false;
    }

    private void IgnoreOldRelationship(Santa_PartnerLink dbRelationship)
    {
        if (_currentUserSuggested)
        {
            dbRelationship.SuggestedByIgnoreOld = true;
        }
        else
        {
            dbRelationship.Confirmed = true;
            dbRelationship.ConfirmingUserIgnore = true;
        }

        bool confirmedOld = dbRelationship.SuggestedByIgnoreOld
            && (dbRelationship.Confirmed == true || dbRelationship.ConfirmingUserIgnore);

        if (confirmedOld)
        {
            if (dbRelationship.Confirmed == true)
            {
                _headerText = "Your old relationship will be ignored";
                _messageText = $"{_dbCurrentUser?.FullName()} confirmed that they're happy to ignore " +
                    $"their old relationship with you, so you can now exchange presents again.";
            }

            dbRelationship.DateArchived ??= DateTime.Now;
            dbRelationship.ExchangeGifts = true;
        }
        else if (dbRelationship.Confirmed == true && dbRelationship.RelationshipEnded != null) // otherwise, other messages will handle this
        {
            _headerText = "Can your old relationship be ignored?";
            _messageText = $"{_dbCurrentUser?.FullName()} said that they're happy to ignore their old " +
                $"relationship with you, so you could exchange presents again. If you're happy to " +
                $"ignore it too, please go to <a href=\"{Item.ManageRelationshipsLink}\">'Manage Your Relationships'</a> " +
                $"to confirm.";
        }

        dbRelationship.RelationshipEnded ??= DateTime.Now;
    }

    private void IgnoreNonRelationship(Santa_PartnerLink dbRelationship)
    {
        if (_currentUserSuggested)
        {
            AddInvalidStatusError();
            return;
        }

        dbRelationship.Confirmed = false;
        dbRelationship.ConfirmingUserIgnore = true;

        if (dbRelationship.RelationshipEnded == null || dbRelationship.SuggestedByIgnoreOld) // otherwise wait for the person who added the relationship to ignore it
        {
            dbRelationship.DateDeleted = DateTime.Now;
            dbRelationship.ExchangeGifts = true;

            if (dbRelationship.RelationshipEnded == null)
            {
                _headerText = "Your suggested relationship was not confirmed";
                _messageText = $"{_dbCurrentUser?.FullName()} denied that they're in a relationship with you. " +
                    $"Sorry if there was a misunderstanding.";
            }
        }

        ArchiveAnyOtherRelationships(); // just in case
    }

    private void AvoidRelationship(Santa_PartnerLink dbRelationship)
    {
        if (_currentUserSuggested || dbRelationship.Confirmed == true || dbRelationship.RelationshipEnded != null)
        {
            AddInvalidStatusError();
            return;
        }

        _headerText = "Your suggested relationship was not confirmed";
        _messageText = $"{_dbCurrentUser?.FullName()} denied that they're in a relationship with you. " +
            $"Sorry if there was a misunderstanding.";

        dbRelationship.DateDeleted = dbRelationship.DateArchived = null; // don't ignore this relationship
        dbRelationship.ConfirmingUserIgnore = false; // just in case
        dbRelationship.Confirmed = false;
    }

    private void ArchiveAnyOtherRelationships()
    {
        var dbOtherRelationships = _dbPossibleRelationships
            .Where(x => x.PartnerLinkKey != Item.PartnerLinkKey)
            .ToList();

        foreach (var dbOtherRelationship in dbOtherRelationships)
        {
            dbOtherRelationship.RelationshipEnded ??= DateTime.Now; // just in case
            dbOtherRelationship.ConfirmingUserIgnore = true;
            dbOtherRelationship.SuggestedByIgnoreOld = true;
            dbOtherRelationship.DateArchived = DateTime.Now;
        }
    }

    private void AddInvalidStatusError()
    {
        AddGeneralValidationError("Invalid new status.");
    }

    private void SendUpdateMessage(Santa_PartnerLink dbRelationship)
    {
        if (_dbCurrentUser?.SantaUser == null) // for the compiler
        {
            throw new AccessDeniedException();
        }

        var dbPartner = _currentUserSuggested ? dbRelationship.ConfirmingSantaUser : dbRelationship.SuggestedBySantaUser;

        var message = new SendSantaMessage
        {
            RecipientTypes = MessageRecipientType.PotentialPartner,
            HeaderText = _headerText,
            MessageText = _messageText,
            Important = false,
            ShowAsFromSanta = true
        };

        SendMessage(message, _dbCurrentUser.SantaUser, dbPartner);
    }
}
