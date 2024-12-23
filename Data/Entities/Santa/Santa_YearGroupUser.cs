namespace Data.Entities.Santa;

public class Santa_YearGroupUser
{
    public Santa_YearGroupUser()
    {
        Suggestions = new HashSet<Santa_Suggestion>();
    }

    [Key]
    public int Id { get; set; }

    public int YearId { get; set; }
    public virtual required Santa_GiftingGroupYear Year { get; set; }

    public int SantaUserId { get; set; }
    public virtual required Santa_User SantaUser { get; set; }

    public bool Included { get; set; }

    public int? GivingToUserId { get; set; }
    public virtual Santa_User? GivingToUser { get; set; }

    public virtual ICollection<Santa_Suggestion> Suggestions { get; set; }
}
