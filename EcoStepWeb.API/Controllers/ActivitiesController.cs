using EcoStepWeb.API.DTOs;
using EcoStepWeb.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoStepWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController(IActivityService activityService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ActivityResponse>>> GetAll()
    {
        var activities = await activityService.GetAllAsync();
        return Ok(activities);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ActivityResponse>> GetById(int id)
    {
        var activity = await activityService.GetByIdAsync(id);

        if (activity == null)
            return NotFound();

        return Ok(activity);
    }

    [HttpPost]
    public async Task<ActionResult<ActivityResponse>> Create(ActivityRequest request)
    {
        var createdActivity = await activityService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = createdActivity.Id }, createdActivity);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ActivityResponse>> Update(int id, ActivityRequest request)
    {
        var updatedActivity = await activityService.UpdateAsync(id, request);

        if (updatedActivity is null)
            return NotFound();

        return Ok(updatedActivity);
    }
}
