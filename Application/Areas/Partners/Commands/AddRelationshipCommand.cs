using Application.Areas.Messages.BaseModels;
using Application.Areas.Partners.Queries;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Partners;
using Global.Extensions.Exceptions;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Partners.Commands;

public class AddRelationshipCommand : BaseCommand<IAddRelationship>
{
    public AddRelationshipCommand(IAddRelationship item) : base(item)
    {
    }

    protected async override Task<ICommandResult<IAddRelationship>> HandlePostValidation()
    {
        if (Item.PossiblePartners == null || Item.PossiblePartners.Count() == 0)
        {
            Item.PossiblePartners = await Send(new GetPossiblePartnersQuery());
        }

        IVisibleUser? selectedPartner = Item.PossiblePartners.FirstOrDefault(x => x.GlobalUserId == Item.GlobalUserId.ToString());

        if (selectedPartner == null)
        {
            AddGeneralValidationError("This person could not be selected as a valid partner.");
            return await Result();
        }

        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser);
        if (dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        var dbSelectedUser = GetGlobalUser(Item.GlobalUserId.ToString(), g => g.SantaUser);
        if (dbSelectedUser == null || dbSelectedUser.SantaUser == null)
        {
            AddGeneralValidationError("This person could not be selected as a valid partner.");
            return await Result();
        }

        dbCurrentUser.SantaUser.SuggestedRelationships.Add(new Santa_PartnerLink
        {
            SuggestedBySantaUser = dbCurrentUser.SantaUser,
            ConfirmingSantaUser = dbSelectedUser.SantaUser
        });

        string messageText = $"{dbCurrentUser.FullName()} says they are in a relationship with you. Is it true? Please go to " +
                $"<a href='{Item.ManageRelationshipsLink}'>'Manage Your Relationships'</a> to confirm.";

        var message = new SendSantaMessage
        {
            RecipientTypes = MessageRecipientType.PotentialPartner,
            HeaderText = "Are you in a relationship?",
            MessageText = messageText,
            Important = true,
            ShowAsFromSanta = true
        };

        SendMessage(message, dbCurrentUser.SantaUser, dbSelectedUser.SantaUser);

        return await SaveAndReturnSuccess();
    }
}
