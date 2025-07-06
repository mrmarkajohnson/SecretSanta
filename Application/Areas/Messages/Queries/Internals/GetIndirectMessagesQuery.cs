namespace Application.Areas.Messages.Queries.Internals;

internal sealed class GetIndirectMessagesQuery : GetMessagesBaseQuery<IEnumerable<Santa_Message>>
{
    public Santa_User DbSantaUser { get; }
    
    public GetIndirectMessagesQuery(Santa_User dbSantaUser)
    {
        DbSantaUser = dbSantaUser;
    }

    protected override Task<IEnumerable<Santa_Message>> Handle()
    {
        var indirectMessages = IndirectMessages(DbSantaUser);
        return Result(indirectMessages);
    }
}
