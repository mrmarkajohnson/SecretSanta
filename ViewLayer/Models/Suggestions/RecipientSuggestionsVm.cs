using Global.Abstractions.Areas.Suggestions;

namespace ViewLayer.Models.Suggestions;

public class RecipientSuggestionsVm
{
    public RecipientSuggestionsVm()
    {
        RecipientSuggestions = new List<ISuggestionBase>().AsQueryable();
    }
    
    public RecipientSuggestionsVm(int giftingGroupKey, string hashedRecipientId, 
        IQueryable<ISuggestionBase> recipientSuggestions)
    {
        GiftingGroupKey = giftingGroupKey;
        HashedRecipientId = hashedRecipientId;
        RecipientSuggestions = recipientSuggestions;
    }

    public int GiftingGroupKey { get; set; }
    public string HashedRecipientId { get; set; } = string.Empty;
    public IQueryable<ISuggestionBase> RecipientSuggestions { get; set; }
}
