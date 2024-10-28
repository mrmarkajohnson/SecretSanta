using Global.Helpers;

namespace Application.Santa.Areas.GiftingGroup.Queries;

public class GetUniqueJoinerTokenQuery : BaseQuery<string>
{
    private readonly string? _existingToken;

    public GetUniqueJoinerTokenQuery(string? existingToken = null)
    {
        _existingToken = existingToken;
    }

    protected override Task<string> Handle()
    {
        List<string> existingTokens = DbContext.Santa_GiftingGroups.Select(x => x.JoinerToken).ToList();

        if (!string.IsNullOrWhiteSpace(_existingToken) && IsUnique(_existingToken, existingTokens))
        {
            return Task.FromResult(_existingToken);
        }

        string newToken = "";
        bool isUnique = false;

        while (!isUnique)
        {
            newToken = RandomHelper.RandomAlphanumericCharacters(20);
            isUnique = IsUnique(newToken, existingTokens);
        }

        return Task.FromResult(newToken);
    }

    private bool IsUnique(string joinerToken, List<string> existingTokens)
    {
        return existingTokens.Contains(joinerToken) == false;
    }
}
