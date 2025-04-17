using Application.Areas.Messages.BaseModels;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Partners;
using Global.Extensions.Exceptions;
using static Global.Settings.MessageSettings;
using static Global.Settings.PartnerSettings;

namespace Application.Areas.Partners.Commands.Internal;

public sealed class RelationshipStatusCommand : BaseCommand<IChangeRelationshipStatus>
{
    public RelationshipStatusCommand(IChangeRelationshipStatus item, Global_User dbCurrentUser,
        List<Santa_PartnerLink> dbPossibleRelationships, Santa_PartnerLink dbRelationship)
        : base(item)
    {
        _dbCurrentUser = dbCurrentUser;
        _dbPossibleRelationships = dbPossibleRelationships;
        _dbRelationship = dbRelationship;
    }

    private Global_User _dbCurrentUser;
    private List<Santa_PartnerLink> _dbPossibleRelationships = new();
    private Santa_PartnerLink _dbRelationship;

    private bool _currentUserSuggested;
    private string _headerText = "";
    private string _messageText = "";

    protected async override Task<ICommandResult<IChangeRelationshipStatus>> HandlePostValidation()
    {
        _currentUserSuggested = _dbRelationship.SuggestedBySantaUserKey == _dbCurrentUser.SantaUser?.SantaUserKey;

        HandleStatusChange();

        if (!string.IsNullOrWhiteSpace(_messageText))
        {
            SendUpdateMessage();
        }

        return await SaveAndReturnSuccess();
    }

    private void HandleStatusChange()
    {
        _dbRelationship.ExchangeGifts = false; // as a starting position

        switch (Item.NewStatus)
        {
            case RelationshipStatus.ToBeConfirmed:
            case RelationshipStatus.ToConfirm:
                AddInvalidStatusError();
                break;
            case RelationshipStatus.Active:
                ConfirmRelationship();
                break;
            case RelationshipStatus.Ended:
                EndConfirmedRelationship();
                break;
            case RelationshipStatus.EndedBeforeConfirmation:
                EndBeforeConfirmation();
                break;
            case RelationshipStatus.IgnoreOld:
                IgnoreOldRelationship();
                break;
            case RelationshipStatus.IgnoreNonRelationship:
                IgnoreNonRelationship();
                break;
            case RelationshipStatus.Avoid:
                AvoidRelationship();
                break;
            default: throw new NotImplementedException();
        }
    }

    private void ConfirmRelationship()
    {
        if (_dbRelationship.RelationshipEnded != null) // just in case
        {
            AddGeneralValidationError("You cannot reactivate an old relationship, as your partner needs to confirm that you are now in a relationship again. " +
                "Please mark this one as ended, then create another.");
            return;
        }

        if (_currentUserSuggested && _dbRelationship.Confirmed != true)
        {
            AddInvalidStatusError();
            return;
        }

        _dbRelationship.Confirmed = true;
        ArchiveAnyOtherRelationships();

        _headerText = "Your relationship is confirmed";
        _messageText = $"{_dbCurrentUser.FullName()} confirmed that {_dbCurrentUser.Gender.IsShort()} in a relationship with you. " +
                $"Congratulations!";
    }

    private void EndConfirmedRelationship()
    {
        if (_currentUserSuggested && _dbRelationship.Confirmed != true)
        {
            AddInvalidStatusError();
            return;
        }

        if (_dbRelationship.RelationshipEnded == null)
        {
            _headerText = "Sorry to hear your relationship has ended";
            _messageText = $"Santa is sorry to hear that {_dbCurrentUser.FullName()} said that {_dbCurrentUser.Gender.IsShort()} no " +
                $"longer in a relationship with you. Santa hopes that you're both OK.";
        }

        if (_currentUserSuggested) // may have changed their mind about exchanging gifts 
        {
            _dbRelationship.SuggestedByIgnoreOld = false;
        }
        else
        {
            _dbRelationship.Confirmed = true;
            _dbRelationship.ConfirmingUserIgnore = false;
        }

        _dbRelationship.ExchangeGifts = false; // just in case
        _dbRelationship.RelationshipEnded ??= DateTime.Now;
    }

    private void EndBeforeConfirmation()
    {
        if (!_currentUserSuggested)
        {
            AddInvalidStatusError();
            return;
        }

        _dbRelationship.ExchangeGifts = false; // just in case
        _dbRelationship.RelationshipEnded ??= DateTime.Now;
        _dbRelationship.SuggestedByIgnoreOld = false;
    }

    private void IgnoreOldRelationship()
    {
        if (_currentUserSuggested)
        {
            _dbRelationship.SuggestedByIgnoreOld = true;
        }
        else
        {
            _dbRelationship.Confirmed = true;
            _dbRelationship.ConfirmingUserIgnore = true;
        }

        bool confirmedOld = _dbRelationship.SuggestedByIgnoreOld
            && (_dbRelationship.Confirmed == true || _dbRelationship.ConfirmingUserIgnore);

        if (confirmedOld)
        {
            if (_dbRelationship.Confirmed == true)
            {
                _headerText = "Your old relationship will be ignored";
                _messageText = $"{_dbCurrentUser.FullName()} confirmed that {_dbCurrentUser.Gender.IsShort()} happy to ignore " +
                        $"{_dbCurrentUser.Gender.Posessive()} old relationship with you, so you can now exchange presents again.";
            }

            _dbRelationship.DateArchived ??= DateTime.Now;
            _dbRelationship.ExchangeGifts = true;
        }
        else if (_dbRelationship.Confirmed == true && _dbRelationship.RelationshipEnded != null) // otherwise, other messages will handle this
        {
            _headerText = "Can your old relationship be ignored?";
            _messageText = $"{_dbCurrentUser.FullName()} said that {_dbCurrentUser.Gender.IsShort()} happy to ignore {_dbCurrentUser.Gender.Posessive()} old " +
                    $"relationship with you, so you could exchange presents again. If you're happy to " +
                    $"ignore it too, please go to <a href=\"{Item.ManageRelationshipsLink}\">'Manage Your Relationships'</a> " +
                    $"to confirm.";
        }

        _dbRelationship.RelationshipEnded ??= DateTime.Now;
    }

    private void IgnoreNonRelationship()
    {
        if (_currentUserSuggested)
        {
            AddInvalidStatusError();
            return;
        }

        _dbRelationship.Confirmed = false;
        _dbRelationship.ConfirmingUserIgnore = true;

        if (_dbRelationship.RelationshipEnded == null || _dbRelationship.SuggestedByIgnoreOld) // otherwise wait for the person who added the relationship to ignore it
        {
            _dbRelationship.DateDeleted = DateTime.Now;
            _dbRelationship.ExchangeGifts = true;

            if (_dbRelationship.RelationshipEnded == null)
            {
                _headerText = "Your suggested relationship was not confirmed";
                _messageText = $"{_dbCurrentUser.FullName()} denied that {_dbCurrentUser.Gender.IsShort()} in a relationship with you. " +
                    $"Sorry if there was a misunderstanding.";
            }
        }

        ArchiveAnyOtherRelationships(); // just in case
    }

    private void AvoidRelationship()
    {
        if (_currentUserSuggested || _dbRelationship.Confirmed == true || _dbRelationship.RelationshipEnded != null)
        {
            AddInvalidStatusError();
            return;
        }

        _headerText = "Your suggested relationship was not confirmed";
        _messageText = $"{_dbCurrentUser.FullName()} denied that {_dbCurrentUser.Gender.IsShort()} in a relationship with you. " +
            $"Sorry if there was a misunderstanding.";

        _dbRelationship.DateDeleted = _dbRelationship.DateArchived = null; // don't ignore this relationship
        _dbRelationship.ConfirmingUserIgnore = false; // just in case
        _dbRelationship.Confirmed = false;
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

    private void SendUpdateMessage()
    {
        if (_dbCurrentUser?.SantaUser == null) // for the compiler
        {
            throw new AccessDeniedException();
        }

        var dbPartner = _currentUserSuggested ? _dbRelationship.ConfirmingSantaUser : _dbRelationship.SuggestedBySantaUser;

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
