using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;
using Rindo.Domain.DTO.Tasks;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TasksController(ITaskService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTask(AddTaskDto projectTask)
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

    [HttpPatch("{taskId:guid}")]
    public async Task<IActionResult> UpdateTaskStage(Guid taskId, Guid stageId)
    {
        await service.UpdateTaskStage(taskId, stageId);
        return Ok();
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

    [HttpPut]
    public async Task<IActionResult> UpdateTask(UpdateTaskDto projectTaskDto)
    {
        await service.UpdateTask(projectTaskDto);
        return Ok();
    }
}
