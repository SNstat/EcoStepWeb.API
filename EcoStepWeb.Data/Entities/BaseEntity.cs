using System.ComponentModel.DataAnnotations;

namespace EcoStepWeb.Data.Entities;

public interface IBaseEntity
{
    int Id { get; set; }
}

public class BaseEntity : IBaseEntity
{
    [Key]
    public int Id { get; set; }
}