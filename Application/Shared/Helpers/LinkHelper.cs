using Global.Abstractions.Areas.Messages;

namespace Application.Shared.Helpers;

public static class LinkHelper
{
    public static string DisplayLink(string url, string display, bool addQuotes)
    {
        string quote = addQuotes ? "\"" : "";
        return $"{quote}<a href=\"{url}\">{display}</a>{quote}";
    }

    public static string MessageLink(string url, string display, bool addQuotes, 
        IEmailRecipient? recipient = null, bool skipReadLink = false)
    {
        if (!skipReadLink)
            AddMessageReadLink(ref url, recipient);

        return DisplayLink(url, display, addQuotes);
    }

    /// <summary>
    /// Add message IDs to mark the message as read; the keys can be added later
    /// </summary>
    private static string AddMessageReadLink(ref string url, IEmailRecipient? recipient)
    {
        url += UrlHelper.ParameterDelimiter(url) +
            $"{MessageSettings.FromMessageParameter}={recipient?.MessageKey ?? 0}" +
            $"&{MessageSettings.FromRecipientParameter}={recipient?.MessageRecipientKey ?? 0}";

        return url;
    }
}
