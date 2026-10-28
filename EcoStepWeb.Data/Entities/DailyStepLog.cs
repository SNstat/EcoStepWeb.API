namespace EcoStepWeb.Data.Entities;

public class DailyStepLog : BaseEntity
{
    public DateTime Date { get; set; }

    public int StepCount { get; set; }

    public int AwardedPoints { get; set; } = 0;

    public int UserId { get; set; }
    public virtual User User { get; set; }
}
