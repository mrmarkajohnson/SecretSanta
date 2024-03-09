namespace Data.Entities.Data.Santa;

public class Santa_YearGroupUser
{
    public Santa_YearGroupUser()
    {
        Suggestions = new HashSet<Santa_Suggestion>();
        SentMessages = new HashSet<Santa_Message>();
    }

    [Key]
    public int Id { get; set; }

    public int YearId { get; set; }
    public virtual required Santa_GiftingGroupYear Year { get; set; }

    public int UserId { get; set; }
    public virtual required Santa_User User { get; set; }

    public int? GivingToUserId { get; set; }
    public virtual Santa_User? GivingToUser { get; set; }

    public virtual ICollection<Santa_Suggestion> Suggestions { get; set; }
    public virtual ICollection<Santa_Message> SentMessages { get; set; }
}
