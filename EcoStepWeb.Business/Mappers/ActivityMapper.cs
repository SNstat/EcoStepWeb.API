using EcoStepWeb.Business.DTOs;
using EcoStepWeb.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace EcoStepWeb.Business.Mappers;

public interface IActivityMapper
{
    ActivityDto ToDto(Activity activity);
    IQueryable<ActivityDto> ToDtos(IQueryable<Activity> activities);
    Activity ToEntity(ActivityDto activityDto);
    void UpdateEntity(ActivityDto activityDto, Activity activity);
}

#pragma warning disable RMG012
#pragma warning disable RMG020

[Mapper]
public partial class ActivityMapper : IActivityMapper
{
    [MapProperty(nameof(Activity.Status), nameof(ActivityDto.StatusText))]
    [MapProperty(nameof(Activity.Organizer.UserName), nameof(ActivityDto.OrganizerUserName))]
    public partial ActivityDto ToDto(Activity activity);

    public partial IQueryable<ActivityDto> ToDtos(IQueryable<Activity> activities);

    [MapperIgnoreTarget(nameof(ActivityDto.Id))]
    [MapperIgnoreSource(nameof(ActivityDto.StatusText))]
    [MapperIgnoreSource(nameof(ActivityDto.OrganizerUserName))]
    public partial Activity ToEntity(ActivityDto activityDto);

    [MapperIgnoreTarget(nameof(ActivityDto.Id))]
    [MapperIgnoreTarget(nameof(Activity.OrganizerId))]
    [MapperIgnoreSource(nameof(ActivityDto.StatusText))]
    [MapperIgnoreSource(nameof(ActivityDto.OrganizerUserName))]
    public partial void UpdateEntity(ActivityDto activityDto, [MappingTarget] Activity activity);
}
