using Application.Areas.Partners.BaseModels;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Partners;

namespace Application.Areas.Partners.Queries;

public sealed class GetRelationshipsQuery : BaseQuery<IRelationships>
{
    protected override Task<IRelationships> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GlobalUser,
            g => g.SuggestedRelationships, g => g.ConfirmingRelationships);

        IEnumerable<IRelationship> suggestedRelationships = dbCurrentSantaUser.SuggestedRelationships
            .Where(x => x.DateArchived == null && x.DateDeleted == null
                && (x.SuggestedByIgnoreOld == false || x.Confirmed == true)) // exclude unconfirmed ignored relationships
            .AsQueryable()
            .ProjectTo<SuggestedRelationship>(Mapper.ConfigurationProvider, new { UserKeysForVisibleEmail = dbCurrentSantaUser.UserKeysForVisibleEmail() });

        IEnumerable<IRelationship> confirmingRelationships = dbCurrentSantaUser.ConfirmingRelationships
            .Where(x => x.DateArchived == null && x.DateDeleted == null && x.Confirmed != false)
            .AsQueryable()
            .ProjectTo<ConfirmingRelationship>(Mapper.ConfigurationProvider, new { UserKeysForVisibleEmail = dbCurrentSantaUser.UserKeysForVisibleEmail() });

        IRelationships relationships = new Relationships
        {
            PossibleRelationships = suggestedRelationships.Union(confirmingRelationships).ToList()
        };

        foreach (var relationship in relationships.PossibleRelationships)
        {
            relationship.Partner.UnHash();
        }

        return Result(relationships);
    }
}
