using Application.Areas.Messages.BaseModels;
using Application.Areas.Partners.BaseModels;
using Application.Areas.Partners.Queries;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Partners;
using Global.Extensions.Exceptions;
using System;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Partners.Commands;

public sealed class AddRelationshipCommand : BaseCommand<IAddRelationship>
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

        IVisibleUser? selectedPartner = Item.PossiblePartners.FirstOrDefault(x => x.HashedUserId == Item.HashedUserId);

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

        var dbSelectedUser = GetGlobalUser(Item.GetStringUserId() ?? "", g => g.SantaUser);
        if (dbSelectedUser == null || dbSelectedUser.SantaUser == null)
        {
            AddGeneralValidationError("This person could not be selected as a valid partner.");
            return await Result();
        }

        dbCurrentUser.SantaUser.SuggestedRelationships.Add(new Santa_PartnerLink
        {
            SuggestedBySantaUser = dbCurrentUser.SantaUser,
            ConfirmingSantaUser = dbSelectedUser.SantaUser,
            RelationshipEnded = Item.IsActive ? null : DateTime.Now
        });

        string areOrWere = Item.IsActive ? "are" : "were once";
        string they = dbCurrentUser.Gender.Direct();
        string messageLink = MessageLink(Item.ManageRelationshipsLink, "Manage Your Relationships", true);

        string messageText = $"{dbCurrentUser.DisplayName()} says {they} {areOrWere} in a relationship with you. " +
            $"Is it true? Please go to {messageLink} to confirm.";

        var message = new SendSantaMessage
        {
            RecipientType = MessageRecipientType.PotentialPartner,
            HeaderText = (Item.IsActive ? "Are" : "Were") + " you in a relationship?",
            MessageText = messageText,
            Important = true,
            CanReply = false,
            ShowAsFromSanta = true
        };

        SendMessage(message, dbCurrentUser.SantaUser, dbSelectedUser.SantaUser);

        return await SaveAndReturnSuccess();
    }
}
