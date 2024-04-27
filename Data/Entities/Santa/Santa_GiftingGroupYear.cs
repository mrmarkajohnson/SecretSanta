using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Santa;

public class Santa_GiftingGroupYear : DeletableBaseEntity
{
    public Santa_GiftingGroupYear()
    {
        Users = new HashSet<Santa_YearGroupUser>();
    }

    [Key]
    public int Id { get; set; }

    [Required, Length(4, 4)]
    public int Year { get; set; }

    [Precision(10, 4)]
    public decimal? Limit { get; set; }

    public int GiftingGroupId { get; set; }
    public virtual required Santa_GiftingGroup GiftingGroup { get; set; }

    public virtual ICollection<Santa_YearGroupUser> Users { get; set; }
}
