using Application.Santa.Areas.Partners.Queries;
using Global.Abstractions.Global.Shared;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.Partners.Commands;

public class AddRelationshipCommand : BaseCommand<Guid>
{
    public AddRelationshipCommand(Guid item) : base(item)
    {
    }

    protected async override Task<ICommandResult<Guid>> HandlePostValidation()
    {
        var possiblePartners = await Send(new GetPossiblePartnersQuery());
        IVisibleUser? selectedPartner = possiblePartners.FirstOrDefault(x => x.Id == Item.ToString());

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

        var dbSelectedUser = GetGlobalUser(Item.ToString(), g => g.SantaUser);
        if (dbSelectedUser == null || dbSelectedUser.SantaUser == null)
        {
            AddGeneralValidationError("This person could not be selected as a valid partner.");
            return await Result();
        }

        dbCurrentUser.SantaUser.SuggestedRelationships.Add(new Santa_PartnerLink
        {
            SuggestedBySantaUser = dbCurrentUser.SantaUser,
            ConfirmingSantaUser = dbSelectedUser.SantaUser,

        });

        return await SaveAndReturnSuccess();
    }
}
