using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.Models;
using Rindo.Infrastructure.Models;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskService _service;
    
    public TaskController(ITaskService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(ProjectTask projectTask)
    {
        await _service.CreateTask(projectTask);
        return Ok(projectTask);
    }

    [HttpGet]
    public async Task<IActionResult> GetTasksByProjectId(Guid projectId)
    {
        var result = await _service.GetTasksByProjectId(projectId); 
        return Ok(result);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetTasksByUser(Guid userId)
    {
        var result = await _service.GetTasksByUserId(userId);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var result = await _service.GetTaskById(id);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var result = await _service.DeleteTask(id);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok();
    }

    [HttpPut("{id:guid}/name")]
    public async Task<IActionResult> UpdateName(Guid id, string name)
    {
        var result = await _service.UpdateName(id, name);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok();
    }
    
    [HttpPut("{id:guid}/description")]
    public async Task<IActionResult> UpdateDescription(Guid id, string description)
    {
        var result = await _service.UpdateDescription(id, description);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok();
    }

    [HttpPut("{id:guid}/responsible")]
    public async Task<IActionResult> UpdateResponsible(Guid id, Nullable<Guid> userId)
    {
        var result  = await _service.UpdateResponsible(id, userId);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok();
    }

    [HttpPost("{id:guid}/start")]
    public async Task<IActionResult> UpdateStartDate(Guid id, [FromBody]DateOnly date)
    {
        var result  = await _service.UpdateStartDate(id, date);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok();
    }
    
    [HttpPost("{id:guid}/finish")]
    public async Task<IActionResult> UpdateFinishDate(Guid id, [FromBody]DateOnly date)
    {
        var result  = await _service.UpdateFinishDate(id, date);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok();
    }
    
    [HttpPut("{id:guid}/progress")]
    public async Task<IActionResult> UpdateProgress(Guid id, string number)
    {
        var result  = await _service.UpdateProgress(id, number);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok();
    }
}
