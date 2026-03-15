using Application.Areas.Partners.Commands.Internal;
using Global.Abstractions.Areas.Partners;
using Global.Extensions.Exceptions;

namespace Application.Areas.Partners.Commands;

public sealed class ChangeRelationshipStatusCommand : BaseCommand<IChangeRelationshipStatus>
{
    public ChangeRelationshipStatusCommand(IChangeRelationshipStatus item) : base(item)
    {
    }

    protected async override Task<ICommandResult<IChangeRelationshipStatus>> HandlePostValidation()
    {
        Global_User? dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser!.SuggestedRelationships, g => g.SantaUser!.ConfirmingRelationships);

        if (dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        string? itemUserId = Item.GetStringUserId();

        var dbSuggestedRelationships = dbCurrentUser.SantaUser.SuggestedRelationships
            .Where(PartnerLinkExpressions.IsActive(false))
            .Where(PartnerLinkExpressions.ConfirmingUserIsActive())
            .Where(x => x.ConfirmingSantaUser.GlobalUserId == itemUserId);

        var dbConfirmingRelationships = dbCurrentUser.SantaUser.ConfirmingRelationships
            .Where(PartnerLinkExpressions.IsActive(false))
            .Where(PartnerLinkExpressions.SuggestingUserIsActive())
            .Where(x => x.SuggestedBySantaUser.GlobalUserId == itemUserId);

        List<Santa_PartnerLink> dbPossibleRelationships = dbSuggestedRelationships.Union(dbConfirmingRelationships).ToList();

        Santa_PartnerLink? dbRelationship = dbPossibleRelationships.FirstOrDefault(x => x.PartnerLinkKey == Item.PartnerLinkKey);

        if (dbRelationship == null)
        {
            AddGeneralValidationError("No matching relationship found. Please reload the page.");
            return await Result();
        }

        return await Send(new RelationshipStatusCommand(Item, dbCurrentUser, dbPossibleRelationships, dbRelationship), null);
    }
}
