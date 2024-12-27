using Application.Santa.Areas.Partners.BaseModels;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Global.Partners;

namespace Application.Santa.Areas.Partners.Queries;

public class GetRelationshipsQuery : BaseQuery<IRelationships>
{
    protected override Task<IRelationships> Handle()
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

        return Task.FromResult(relationships);
    }
}
