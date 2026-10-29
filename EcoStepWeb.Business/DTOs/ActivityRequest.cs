using EcoStepWeb.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace EcoStepWeb.API.DTOs;

public record ActivityRequest(
    [Required, StringLength(Constants.SMALLSTRINGLENGHT)] string Title,
    [Required, StringLength(Constants.LARGESTRINGLENGHT)] string Description,
    [Required, StringLength(Constants.MEDIUMSTRINGLENGHT)] string Location,
    [Required] DateTime StartingDateTime,
    [Required] DateTime EndingDateTime,
    [Range(1, 1000)] int PointsForParticipation,
    [Range(1, int.MaxValue)] int? MaxParticipants,
    [Range(1, int.MaxValue)] int? OrganiserId,
    string? Status);