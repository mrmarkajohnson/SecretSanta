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
        Santa_User dbSantaUser = GetCurrentSantaUser(s => s.GlobalUser,
            g => g.SuggestedRelationships, g => g.ConfirmingRelationships);

        IEnumerable<IRelationship> suggestedRelationships = dbSantaUser.SuggestedRelationships
            .Where(x => x.DateArchived == null && x.DateDeleted == null
                && (x.SuggestedByIgnoreOld == false || x.Confirmed == true)) // exclude unconfirmed ignored relationships
            .AsQueryable()
            .ProjectTo<SuggestedRelationship>(Mapper.ConfigurationProvider, new { UserKeysForVisibleEmail = dbSantaUser.UserKeysForVisibleEmail() });

        IEnumerable<IRelationship> confirmingRelationships = dbSantaUser.ConfirmingRelationships
            .Where(x => x.DateArchived == null && x.DateDeleted == null && x.Confirmed != false)
            .AsQueryable()
            .ProjectTo<ConfirmingRelationship>(Mapper.ConfigurationProvider, new { UserKeysForVisibleEmail = dbSantaUser.UserKeysForVisibleEmail() });

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
