using Application.Santa.Areas.Account.Actions;
using Application.Santa.Areas.Partners.BaseModels;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Global.Partners;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.Partners.Queries;

public class GetRelationshipsQuery : BaseQuery<IRelationships>
{
    protected async override Task<IRelationships> Handle()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, 
            g => g.SantaUser.SuggestedRelationships, g => g.SantaUser.ConfirmingRelationships);

        if (dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        IEnumerable<IRelationship> suggestedRelationships = dbCurrentUser.SantaUser.SuggestedRelationships
            .Where(x => x.DateArchived == null && x.DateDeleted == null)
            .AsQueryable()
            .ProjectTo<SuggestedRelationship>(Mapper.ConfigurationProvider);

        IEnumerable<IRelationship> confirmingRelationships = dbCurrentUser.SantaUser.ConfirmingRelationships
            .Where(x => x.DateArchived == null && x.DateDeleted == null)
            .AsQueryable()
            .ProjectTo<ConfirmingRelationship>(Mapper.ConfigurationProvider);

        IRelationships relationships = new Relationships
        {
            PossibleRelationships = suggestedRelationships.Union(confirmingRelationships).ToList()
        };

        foreach (var relationship in relationships.PossibleRelationships)
        {
            await Send(new UnHashUserIdentificationAction(relationship.Partner));
        }

        return relationships;
    }
}
