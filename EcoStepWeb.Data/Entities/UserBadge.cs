namespace EcoStepWeb.Data.Entities;

public class UserBadge : BaseEntity
{
    public DateTime AwardDateTime { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
    public virtual User User { get; set; }

    public int BadgeId { get; set; }
    public virtual Badge Badge { get; set; }
}
