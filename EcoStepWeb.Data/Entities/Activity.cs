using EcoStepWeb.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace EcoStepWeb.Data.Entities;

public class Activity : BaseEntity
{
    [StringLength(Constants.SMALLSTRINGLENGHT)]
    public string Title { get; set; } = string.Empty;

    [StringLength(Constants.LARGESTRINGLENGHT)]
    public string Description { get; set; } = string.Empty;

    [StringLength(Constants.MEDIUMSTRINGLENGHT)]
    public string Location { get; set; } = string.Empty;

    public DateTime StartingDateTime { get; set; }

    public DateTime EndingDateTime { get; set; }

    public int PointsForParticipation { get; set; } = 0;

    public int? MaxParticipants { get; set; }

    public ActivityStatus Status { get; set; } = ActivityStatus.Registered;

    public int OrganizerId { get; set; }
    public virtual User Organizer { get; set; }

    public virtual ICollection<ActivityRegistration> ActivityRegistrations { get; set; } = [];
}
