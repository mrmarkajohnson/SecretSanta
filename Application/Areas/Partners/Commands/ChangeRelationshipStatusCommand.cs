using Application.Areas.Partners.Commands.Internal;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Partners;
using Global.Extensions.Exceptions;

namespace Application.Areas.Partners.Commands;

public class ChangeRelationshipStatusCommand : BaseCommand<IChangeRelationshipStatus>
{
    public ChangeRelationshipStatusCommand(IChangeRelationshipStatus item) : base(item)
    {
    }

    protected async override Task<ICommandResult<IChangeRelationshipStatus>> HandlePostValidation()
    {
        Global_User? dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser,
            g => g.SantaUser.SuggestedRelationships, g => g.SantaUser.ConfirmingRelationships);

        if (dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        List<Santa_PartnerLink> dbPossibleRelationships = dbCurrentUser.SantaUser.SuggestedRelationships
            .Where(x => x.DateArchived == null && x.DateDeleted == null
                && x.ConfirmingSantaUser.GlobalUserId == Item.GlobalUserId.ToString())
            .Union(dbCurrentUser.SantaUser.ConfirmingRelationships
                .Where(x => x.DateArchived == null && x.DateDeleted == null
                    && x.SuggestedBySantaUser.GlobalUserId == Item.GlobalUserId.ToString()))
            .ToList();

        Santa_PartnerLink? dbRelationship = dbPossibleRelationships.FirstOrDefault(x => x.PartnerLinkKey == Item.PartnerLinkKey);

        if (dbRelationship == null)
        {
            AddGeneralValidationError("No matching relationship found. Please reload the page.");
            return await Result();
        }

        return await Send(new RelationshipStatusCommand(Item, dbCurrentUser, dbPossibleRelationships, dbRelationship), null);
    }
}
