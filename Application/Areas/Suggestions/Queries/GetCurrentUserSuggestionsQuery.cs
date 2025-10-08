using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Suggestions;

namespace Application.Areas.Suggestions.Queries;

public class GetCurrentUserSuggestionsQuery : BaseQuery<IQueryable<ISuggestion>>
{
    protected override Task<IQueryable<ISuggestion>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.Suggestions);

        var dbSuggestions = dbCurrentSantaUser.Suggestions
            .Where(x => x.DateDeleted == null && x.DateArchived == null)
            .AsQueryable()
            .ProjectTo<ISuggestion>(Mapper.ConfigurationProvider);

        return Result(dbSuggestions);
    }
}
