using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Suggestions;

namespace Application.Areas.Suggestions.Queries;

public class GetCurrentUserSuggestionsQuery : BaseQuery<IQueryable<ISuggestion>>
{
    protected override Task<IQueryable<ISuggestion>> Handle()
    {
        Santa_User dbSantaUser = GetCurrentSantaUser(s => s.Suggestions);

        var dbSuggestions = dbSantaUser.Suggestions
            .Where(x => x.DateDeleted == null && x.DateArchived == null)
            .AsQueryable()
            .ProjectTo<ISuggestion>(Mapper.ConfigurationProvider);

        return Task.FromResult(dbSuggestions);
    }
}
