using EcoStepWeb.API.DTOs;
using EcoStepWeb.Data;
using EcoStepWeb.Data.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace EcoStepWeb.API.Services;

public interface IActivityService
{
    Task<List<ActivityResponse>> GetAllAsync();
    Task<ActivityResponse?> GetByIdAsync(int id);
    Task<ActivityResponse> CreateAsync(ActivityRequest request);
    Task<ActivityResponse?> UpdateAsync(int id, ActivityRequest request);
}

public class ActivityService(AppDbContext context) : IActivityService
{
    public async Task<List<ActivityResponse>> GetAllAsync()
    {
        var activityResponses = await context.Activities
            .Select(_ => new ActivityResponse(
                _.Id,
                _.Title,
                _.Description,
                _.Location,
                _.StartingDateTime,
                _.EndingDateTime,
                _.PointsForParticipation,
                _.MaxParticipants,
                _.Status.ToString(),
                _.Organizer.UserName)
        ).ToListAsync();

        return activityResponses;
    }

    public async Task<ActivityResponse?> GetByIdAsync(int id)
    {
        var activityResponse = await context.Activities
            .Where(_ => _.Id == id)
            .Select(_ => new ActivityResponse(
                _.Id,
                _.Title,
                _.Description,
                _.Location,
                _.StartingDateTime,
                _.EndingDateTime,
                _.PointsForParticipation,
                _.MaxParticipants,
                _.Status.ToString(),
                _.Organizer.UserName))
            .FirstOrDefaultAsync();

        return activityResponse;
    }

    public async Task<ActivityResponse> CreateAsync(ActivityRequest request)
    {
        var newActivity = new Data.Entities.Activity
        {
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            StartingDateTime = request.StartingDateTime,
            EndingDateTime = request.EndingDateTime,
            PointsForParticipation = request.PointsForParticipation,
            MaxParticipants = request.MaxParticipants,
            OrganizerId = (int)request.OrganiserId!
        };

        context.Activities.Add(newActivity);
        await context.SaveChangesAsync();

        var organizerName = await context.Users
            .Where(_ => _.Id == request.OrganiserId)
            .Select(_ => _.UserName)
            .FirstAsync();

        var newActivityResponse = new ActivityResponse(
            newActivity.Id,
            newActivity.Title,
            newActivity.Description,
            newActivity.Location,
            newActivity.StartingDateTime,
            newActivity.EndingDateTime,
            newActivity.PointsForParticipation,
            newActivity.MaxParticipants,
            newActivity.Status.ToString(),
            organizerName);

        return newActivityResponse;
    }

    public async Task<ActivityResponse?> UpdateAsync(int id, ActivityRequest request)
    {
        var activity = await context.Activities
            .Include(_ => _.Organizer)
            .FirstOrDefaultAsync(_ => _.Id == id);

        if (activity == null) {
            return null!;
        }

        activity.Title = request.Title;
        activity.Description = request.Description;
        activity.Location = request.Location;
        activity.StartingDateTime = request.StartingDateTime;
        activity.EndingDateTime = request.EndingDateTime;
        activity.PointsForParticipation = request.PointsForParticipation;
        activity.MaxParticipants = request.MaxParticipants;
        
        if(request.Status != null)
        {
            activity.Status = Enum.Parse<ActivityStatus>(request.Status, ignoreCase: true);
        }

        await context.SaveChangesAsync();

        var newActivityResponse = new ActivityResponse(
            activity.Id,
            activity.Title,
            activity.Description,
            activity.Location,
            activity.StartingDateTime,
            activity.EndingDateTime,
            activity.PointsForParticipation,
            activity.MaxParticipants,
            activity.Status.ToString(),
            activity.Organizer.UserName);

        return newActivityResponse;
    }
}
