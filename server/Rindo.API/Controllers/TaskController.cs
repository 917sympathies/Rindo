using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.Models;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TaskController(ITaskService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTask(ProjectTask projectTask)
    {
        await service.CreateTask(projectTask);
        return Ok(projectTask);
    }

    [HttpGet]
    public async Task<IActionResult> GetTasksByProjectId(Guid projectId)
    {
        var result = await service.GetTasksByProjectId(projectId); 
        return Ok(result);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetTasksByUser(Guid userId)
    {
        var result = await service.GetTasksByUserId(userId);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var result = await service.GetTaskById(id);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        await service.DeleteTask(id);
        return Ok();
    }

    [HttpPut("{id:guid}/name")]
    public async Task<IActionResult> UpdateName(Guid id, string name)
    {
        await service.UpdateName(id, name);
        return Ok();
    }
    
    [HttpPut("{id:guid}/description")]
    public async Task<IActionResult> UpdateDescription(Guid id, string description)
    {
        await service.UpdateDescription(id, description);
        return Ok();
    }

    [HttpPut("{id:guid}/responsible")]
    public async Task<IActionResult> UpdateResponsible(Guid id, Nullable<Guid> userId)
    {
        await service.UpdateResponsible(id, userId);
        return Ok();
    }

    [HttpPost("{id:guid}/start")]
    public async Task<IActionResult> UpdateStartDate(Guid id, [FromBody]DateOnly date)
    {
        await service.UpdateStartDate(id, date);
        return Ok();
    }
    
    [HttpPost("{id:guid}/finish")]
    public async Task<IActionResult> UpdateFinishDate(Guid id, [FromBody]DateOnly date)
    {
        await service.UpdateFinishDate(id, date);
        return Ok();
    }
    
    [HttpPut("{id:guid}/progress")]
    public async Task<IActionResult> UpdateProgress(Guid id, string number)
    {
        await service.UpdateProgress(id, number);
        return Ok();
    }
}
