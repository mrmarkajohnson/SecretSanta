using Application.Areas.Partners.BaseModels;
using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Partners;
using Global.Extensions.Exceptions;

namespace Application.Areas.Partners.Queries;

public sealed class GetRelationshipsQuery : BaseQuery<IRelationships>
{
    protected override Task<IRelationships> Handle()
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
            .Where(x => x.DateArchived == null && x.DateDeleted == null && x.Confirmed != false)
            .AsQueryable()
            .ProjectTo<ConfirmingRelationship>(Mapper.ConfigurationProvider);

        IRelationships relationships = new Relationships
        {
            PossibleRelationships = suggestedRelationships.Union(confirmingRelationships).ToList()
        };

        foreach (var relationship in relationships.PossibleRelationships)
        {
            relationship.Partner.UnHash();
        }

        return Task.FromResult(relationships);
    }
}
