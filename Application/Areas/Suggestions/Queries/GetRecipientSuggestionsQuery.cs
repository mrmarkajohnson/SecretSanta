using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Suggestions;
using Global.Extensions.Exceptions;
namespace Application.Areas.Suggestions.Queries;

public sealed class GetRecipientSuggestionsQuery : BaseQuery<IQueryable<ISuggestionBase>>
{
    public int GiftingGroupKey { get; }
    public string HashedUserId { get; }
    public int CalendarYear { get; }

    public GetRecipientSuggestionsQuery(int giftingGroupKey, string hashedUserId, int calendarYear)
	{
        GiftingGroupKey = giftingGroupKey;
        HashedUserId = hashedUserId;
        CalendarYear = calendarYear;
    }

    protected override Task<IQueryable<ISuggestionBase>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks, s => s.GiftingGroupYears);

        Santa_GiftingGroup dbGroup = dbCurrentSantaUser.GiftingGroupLinks
            .Where(GroupUserExpressions.IsActive(true))
            .Select(x => x.GiftingGroup)
            .FirstOrDefault(y => y.GiftingGroupKey == GiftingGroupKey)
        ?? throw new NotFoundException("Group");

        Guid userId = UserHelper.GetGlobalUserId(HashedUserId) ?? new Guid();

        var dbYearGroupUser = dbGroup.Years
            .Where(GroupYearExpressions.IsActive(false))
            .Where(x => x.CalendarYear == CalendarYear)
            .SelectMany(x => x.Users)
                .Where(YearGroupUserExpressions.IsActive(true))
                .FirstOrDefault(y => y.SantaUser.GlobalUserId == userId.ToString())
        ?? throw new NotFoundException("User");

        var dbSuggestions = dbYearGroupUser.Suggestions
           .Where(SuggestionLinkExpressions.IsActive(false))           
           .Select(x => x.Suggestion)
           .AsQueryable()
           .ProjectTo<ISuggestionBase>(Mapper.ConfigurationProvider);

        return Result(dbSuggestions);
    }
}