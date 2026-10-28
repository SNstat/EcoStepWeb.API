using EcoStepWeb.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace EcoStepWeb.Data.Entities;

public class User : BaseEntity
{
    [StringLength(Constants.SMALLSTRINGLENGHT)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(Constants.SMALLSTRINGLENGHT)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(Constants.SMALLSTRINGLENGHT)]
    public string UserName { get; set; } = string.Empty;

    [StringLength(Constants.SMALLSTRINGLENGHT)]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public int TotalEcoPoints { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public UserRole UserRole { get; set; }

    public virtual ICollection<ActivityRegistration> ActivityRegistrations { get; set; } = [];

    public virtual ICollection<UserBadge> UserBadges { get; set; } = [];
}
