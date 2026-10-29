using EcoStepWeb.API.Services;
using EcoStepWeb.Business.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EcoStepWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class activityController(IActivityService activityService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ActivityDto>>> GetAll()
    {
        var activities = await activityService.GetAllAsync();
        return Ok(activities);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ActivityDto>> GetById(int id)
    {
        var activity = await activityService.GetByIdAsync(id);

        if (activity == null)
            return NotFound();

        return Ok(activity);
    }

    [HttpPost]
    public async Task<ActionResult<ActivityDto>> Create(ActivityDto request)
    {
        var createdActivity = await activityService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = createdActivity.Id }, createdActivity);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ActivityDto>> Update(int id, ActivityDto request)
    {
        var updatedActivity = await activityService.UpdateAsync(id, request);

        if (updatedActivity is null)
            return NotFound();

        return Ok(updatedActivity);
    }
}
