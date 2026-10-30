using EcoStepWeb.Business.DTOs;
using EcoStepWeb.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace EcoStepWeb.Business.Mappers;

#pragma warning disable RMG012
#pragma warning disable RMG020
#pragma warning disable RMG065

[Mapper]
public partial class ActivityMapper
{
    [MapProperty(nameof(Activity.Status), nameof(ActivityDto.StatusText))]
    [MapProperty(nameof(Activity.Organizer.UserName), nameof(ActivityDto.OrganizerUserName))]
    public partial ActivityDto MapActivityToDto(Activity activity);

    public partial IQueryable<ActivityDto> MapActivitiesToDtos(IQueryable<Activity> activities);

    [MapperIgnoreTarget(nameof(ActivityDto.Id))]
    [MapperIgnoreSource(nameof(ActivityDto.StatusText))]
    [MapperIgnoreSource(nameof(ActivityDto.OrganizerUserName))]
    public partial Activity MapDtoToActivity(ActivityDto activityDto);

    [MapperIgnoreTarget(nameof(ActivityDto.Id))]
    [MapperIgnoreTarget(nameof(Activity.OrganizerId))]
    [MapperIgnoreSource(nameof(ActivityDto.StatusText))]
    [MapperIgnoreSource(nameof(ActivityDto.OrganizerUserName))]
    public partial void MapUpdateActivityWithDto(ActivityDto activityDto, [MappingTarget] Activity activity);
}