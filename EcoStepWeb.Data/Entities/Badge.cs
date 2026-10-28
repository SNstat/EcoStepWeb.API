using System.ComponentModel.DataAnnotations;

namespace EcoStepWeb.Data.Entities;

public class Badge : BaseEntity
{
    [StringLength(Constants.SMALLSTRINGLENGHT)]
    public string Name { get; set; } = string.Empty;

    [StringLength(Constants.MEDIUMSTRINGLENGHT)]
    public string Description { get; set; } = string.Empty;

    [StringLength(Constants.MEDIUMSTRINGLENGHT)]
    public string? IconUrl { get; set; }

    public virtual ICollection<UserBadge> UserBadges { get; set; } = [];
}
