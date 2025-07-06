using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Suggestions;
using Global.Extensions.Exceptions;
namespace Application.Areas.Suggestions.Queries;

public class GetRecipientSuggestionsQuery : BaseQuery<IQueryable<ISuggestionBase>>
{
    public int GiftingGroupKey { get; }
    public string HashedUserId { get; }
    
    public GetRecipientSuggestionsQuery(int giftingGroupKey, string hashedUserId)
	{
        GiftingGroupKey = giftingGroupKey;
        HashedUserId = hashedUserId;
    }

    protected override Task<IQueryable<ISuggestionBase>> Handle()
    {
        Santa_User dbSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks, s => s.GiftingGroupYears);

        Santa_GiftingGroup dbGroup = dbSantaUser.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.DateArchived == null)
            .Select(x => x.GiftingGroup)
            .Where(y => y.DateDeleted == null && y.DateArchived == null)
            .FirstOrDefault(y => y.GiftingGroupKey == GiftingGroupKey)
        ?? throw new NotFoundException("Group");

        Guid userId = UserHelper.GetGlobalUserId(HashedUserId) ?? new Guid();

        var dbYearGroupUser = dbGroup.Years
            .Where(x => x.CalendarYear == DateTime.Today.Year)
            .SelectMany(x => x.Users)
            .FirstOrDefault(y => y.SantaUser.GlobalUserId == userId.ToString())
        ?? throw new NotFoundException("User");

        var dbSuggestions = dbYearGroupUser.Suggestions
           .Where(x => x.DateDeleted == null && x.DateArchived == null)           
           .Select(x => x.Suggestion)
           .AsQueryable()
           .ProjectTo<ISuggestionBase>(Mapper.ConfigurationProvider);

        return Result(dbSuggestions);
    }
}