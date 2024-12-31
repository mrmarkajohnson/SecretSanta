using Application.Santa.Areas.Account.Actions;
using Application.Santa.Areas.Partners.BaseModels;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Global.Partners;

namespace Application.Santa.Areas.Partners.Queries;

public class GetRelationshipsQuery : BaseQuery<IRelationships>
{
    protected async override Task<IRelationships> Handle()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, 
            g => g.SantaUser.SuggestedRelationships, g => g.SantaUser.ConfirmingRelationships);

        IEnumerable<IRelationship> suggestedRelationships = dbCurrentUser.SantaUser.SuggestedRelationships
            .AsQueryable()
            .ProjectTo<SuggestedRelationship>(Mapper.ConfigurationProvider);

        IEnumerable<IRelationship> confirmingRelationships = dbCurrentUser.SantaUser.ConfirmingRelationships
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
