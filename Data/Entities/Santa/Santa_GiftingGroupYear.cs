using Global.Abstractions.Santa.Areas.GiftingGroup;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Santa;

public class Santa_GiftingGroupYear : DeletableBaseEntity, IGiftingGroupYearBase
{
    public Santa_GiftingGroupYear()
    {
        Users = new HashSet<Santa_YearGroupUser>();
    }

    [Key]
    public int Id { get; set; }

    [Required, Length(4, 4)]
    public int Year { get; set; }

    [Precision(10, 2)]
    public decimal? Limit { get; set; }

    public int GiftingGroupId { get; set; }
    public virtual required Santa_GiftingGroup GiftingGroup { get; set; }

    public virtual ICollection<Santa_YearGroupUser> Users { get; set; }
}
