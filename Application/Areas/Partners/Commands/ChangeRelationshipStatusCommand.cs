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

    protected async override Task<ICommandResult<IChangeRelationshipStatus>> HandlePostValidation()
    {
        _dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser,
            g => g.SantaUser.SuggestedRelationships, g => g.SantaUser.ConfirmingRelationships);

        if (_dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        var dbPossibleRelationships = _dbCurrentUser.SantaUser.SuggestedRelationships
            .Where(x => x.DateArchived == null && x.DateDeleted == null
                && x.ConfirmingSantaUser.GlobalUserId == Item.UserId.ToString())
            .Union(_dbCurrentUser.SantaUser.ConfirmingRelationships
                .Where(x => x.DateArchived == null && x.DateDeleted == null
                    && x.SuggestedBySantaUser.GlobalUserId == Item.UserId.ToString()));

        Santa_PartnerLink? dbRelationship = dbPossibleRelationships.FirstOrDefault(x => x.Id == Item.PartnerLinkId);

        if (dbRelationship == null)
        {
            AddGeneralValidationError("No matching relationship found. Please reload the page.");
            return await Result();
        }

        bool currentUserSuggested = dbRelationship.SuggestedBySantaUserId == _dbCurrentUser.SantaUser.Id;
        string headerText = "";
        string messageText = "";

        switch (Item.NewStatus)
        {
            case RelationshipStatus.ToBeConfirmed:
            case RelationshipStatus.ToConfirm:
                AddGeneralValidationError("Invalid new status.");
                return await Result();
            case RelationshipStatus.Active:
                ConfirmRelationship(dbPossibleRelationships, dbRelationship, ref headerText, ref messageText);
                break;
            case RelationshipStatus.Ended:
                dbRelationship.RelationshipEnded ??= DateTime.Now;
                headerText = "Sorry to hear your relationship has ended";
                messageText = $"Santa is sorry to hear that {_dbCurrentUser.FullName()} said that they're no " +
                    $"longer in a relationship with you. He hopes that you're both OK.";
                break;
            case RelationshipStatus.EndedBeforeConfirmation:
                dbRelationship.RelationshipEnded ??= DateTime.Now;
                break;
            case RelationshipStatus.IgnoreOld:
                IgnoreOldRelationship(dbRelationship, currentUserSuggested, ref headerText, ref messageText);

                break;
            case RelationshipStatus.NotRelationship:
                dbRelationship.DateDeleted = DateTime.Now;
                headerText = "Your suggested relationship was not confirmed";
                messageText = $"{_dbCurrentUser.FullName()} denied that they're in a relationship with you. " +
                    $"Sorry if there was a misunderstanding.";
                break;
            default: throw new NotImplementedException();
        }

        if (!string.IsNullOrWhiteSpace(messageText))
        {
            SendUpdateMessage(dbRelationship, currentUserSuggested, headerText, messageText);
        }

        return await SaveAndReturnSuccess();
    }

    private void ConfirmRelationship(IEnumerable<Santa_PartnerLink> dbPossibleRelationships, Santa_PartnerLink dbRelationship,
        ref string headerText, ref string messageText)
    {
        dbRelationship.Confirmed = true;
        headerText = "Your relationship is confirmed";
        messageText = $"{_dbCurrentUser?.FullName()} confirmed that they're in a relationship with you. " +
            $"Congratulations!";
        ArchiveAnyOtherRelationships(dbPossibleRelationships);
    }

    private void IgnoreOldRelationship(Santa_PartnerLink dbRelationship, bool currentUserSuggested,
        ref string headerText, ref string messageText)
    {
        dbRelationship.RelationshipEnded ??= DateTime.Now; // just in case                

        if (currentUserSuggested)
        {
            dbRelationship.SuggestedByIgnoreOld = true;
        }
        else
        {
            dbRelationship.ConfirmedByIgnoreOld = true;
        }

        bool confirmedOld = dbRelationship.SuggestedByIgnoreOld
            && (!dbRelationship.Confirmed || dbRelationship.ConfirmedByIgnoreOld);

        if (confirmedOld)
        {
            dbRelationship.DateArchived ??= DateTime.Now;

            if (dbRelationship.Confirmed)
            {
                headerText = "Your old relationship will be ignored";
                messageText = $"{_dbCurrentUser?.FullName()} confirmed that they're happy to ignore " +
                    $"their old relationship with you, so you can now exchange presents again.";
            }
        }
        else
        {
            headerText = "Can your old relationship be ignored?";
            messageText = $"{_dbCurrentUser?.FullName()} said that they're happy to ignore their old " +
                $"relationship with you, so you could exchange presents again. If you're happy to " +
                $"ignore it too, please go to <a href='{Item.ManageRelationshipsLink}'>'Manage Your Relationships'</a> " +
                $"to confirm.";
        }
    }

    private void ArchiveAnyOtherRelationships(IEnumerable<Santa_PartnerLink> dbPossibleRelationships)
    {
        var dbOtherRelationships = dbPossibleRelationships
                        .Where(x => x.Id != Item.PartnerLinkId)
                        .ToList();

        foreach (var dbOtherRelationship in dbOtherRelationships)
        {
            dbOtherRelationship.RelationshipEnded ??= DateTime.Now; // just in case
            dbOtherRelationship.ConfirmedByIgnoreOld = true;
            dbOtherRelationship.SuggestedByIgnoreOld = true;
            dbOtherRelationship.DateArchived = DateTime.Now;
        }
    }

    private void SendUpdateMessage(Santa_PartnerLink dbRelationship, bool currentUserSuggested,
        string headerText, string messageText)
    {
        if (_dbCurrentUser?.SantaUser == null) // for the compiler
        {
            throw new AccessDeniedException();
        }

        var dbPartner = currentUserSuggested ? dbRelationship.ConfirmingSantaUser : dbRelationship.SuggestedBySantaUser;

        var message = new SendSantaMessage
        {
            RecipientTypes = MessageRecipientType.PotentialPartner,
            HeaderText = headerText,
            MessageText = messageText,
            Important = false,
            ShowAsFromSanta = true
        };

        SendMessage(message, _dbCurrentUser.SantaUser, dbPartner);
    }
}
