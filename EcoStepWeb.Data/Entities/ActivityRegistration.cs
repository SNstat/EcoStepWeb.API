using EcoStepWeb.Data.Enumerations;

namespace EcoStepWeb.Data.Entities;

public class ActivityRegistration : BaseEntity
{
    public DateTime RegistrationDateTime { get; set; } = DateTime.UtcNow;

    public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.Registered;

    public int UserId { get; set; }
    public virtual User User { get; set; }

    public int ActivityId { get; set; }
    public virtual Activity Activity { get; set; }
}
