using EcoStepWeb.Data.Enumerations;

namespace EcoStepWeb.Business.DTOs;

public class ActivityDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartingDateTime { get; set; }
    public DateTime EndingDateTime { get; set; }
    public int PointsForParticipation { get; set; }
    public int? MaxParticipants { get; set; }

    public ActivityStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;

    public int OrganizerId { get; set; }
    public string OrganizerUserName { get; set; } = string.Empty;
}
