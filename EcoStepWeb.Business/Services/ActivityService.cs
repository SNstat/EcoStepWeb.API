using EcoStepWeb.Business.DTOs;
using EcoStepWeb.Business.Mappers;
using EcoStepWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace EcoStepWeb.API.Services;

public interface IActivityService
{
    Task<List<ActivityDto>> GetAllAsync();
    Task<ActivityDto?> GetByIdAsync(int id);
    Task<ActivityDto> CreateAsync(ActivityDto dto);
    Task<ActivityDto?> UpdateAsync(int id, ActivityDto dto);
}

public class ActivityService(AppDbContext context, IActivityMapper activityMapper) : IActivityService
{
    public Task<List<ActivityDto>> GetAllAsync()
        => activityMapper.ToDtos(context.Activities).ToListAsync();

    public Task<ActivityDto?> GetByIdAsync(int id)
        => activityMapper.ToDtos(context.Activities.Where(a => a.Id == id)).FirstOrDefaultAsync();


    public async Task<ActivityDto> CreateAsync(ActivityDto dto)
    {
        var newActivity = activityMapper.ToEntity(dto);

        context.Activities.Add(newActivity);

        await context.SaveChangesAsync();

        var responseDto = await activityMapper.ToDtos(context.Activities
                               .Where(a => a.Id == newActivity.Id))
                               .FirstAsync();

        return responseDto;
    }

    public async Task<ActivityDto?> UpdateAsync(int id, ActivityDto dto)
    {
        var targetActivity = await context.Activities
            .Include(_ => _.Organizer)
            .FirstOrDefaultAsync(_ => _.Id == id);

        if (targetActivity is null)
            return null;

        activityMapper.UpdateEntity(dto, targetActivity);

        await context.SaveChangesAsync();

        var responseDto = activityMapper.ToDto(targetActivity);

        return responseDto;
    }
}
