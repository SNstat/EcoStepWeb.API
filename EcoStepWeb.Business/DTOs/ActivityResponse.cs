namespace EcoStepWeb.API.DTOs;

public record ActivityResponse(
    int Id,
    string Title,
    string Description,
    string Location,
    DateTime StartingDateTime,
    DateTime EndingDateTime,
    int PointsForParticipation,
    int? MaxParticipants,
    string Status,
    string OrganizerUserName);