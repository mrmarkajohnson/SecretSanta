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
            .Where(DbPartnerLinkExpressions.IsActive())
            .Where(x => x.SuggestedBySantaUser.DateArchived == null)
            .Where(x => x.SuggestedByIgnoreOld == false || x.Confirmed == true) // exclude unconfirmed ignored relationships
            .AsQueryable()
            .ProjectTo<SuggestedRelationship>(Mapper.ConfigurationProvider, new { UserKeysForVisibleEmail = dbCurrentSantaUser.UserKeysForVisibleEmail() });

        IEnumerable<IRelationship> confirmingRelationships = dbCurrentSantaUser.ConfirmingRelationships
            .Where(DbPartnerLinkExpressions.IsActive())
            .Where(x => x.ConfirmingSantaUser.DateArchived == null)
            .Where(x => x.Confirmed != false || !x.ConfirmingUserIgnore)
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
