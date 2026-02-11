using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProjectsController(IProjectService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] ProjectOnCreateDto projectOnCreateDto)
    {
        var project = await service.CreateProject(projectOnCreateDto);
        return Ok(project);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProjectById(Guid id)
    {
        var project = await service.GetProjectById(id);
        return Ok(project);
    }
    
    [HttpGet("{id:guid}/settings")]
    public async Task<IActionResult> GetProjectSettings(Guid id)
    {
        var project = await service.GetProjectSettings(id);
        return Ok(project);
    }

    [HttpGet]
    public async Task<IActionResult> GetProjectsWhereUserAttends(Guid userId)
    {
        return Ok(await service.GetProjectsWhereUserAttends(userId));
    }
    
    [HttpGet("{id:guid}/header")]
    public async Task<IActionResult> GetProjectsInfoForHeader(Guid id)
    {
        return Ok(await service.GetProjectsInfoForHeader(id));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await service.DeleteProject(id);
        return Ok();
    }

    [HttpPost("{id:guid}/invite")]
    public async Task<IActionResult> InviteUserToProject(Guid id, string username)
    {
        await service.InviteUserToProject(id, username);
        return Ok();
    }
    
    [HttpPost("{id:guid}")]
    public async Task<IActionResult> AddUserToProject(Guid id, Guid userId)
    {
        await service.AddUserToProject(id, userId);
        return Ok();
    }

    [HttpPost("{id:guid}/remove")]
    public async Task<IActionResult> RemoveUserFromProject(Guid id, string username)
    {
        await service.RemoveUserFromProject(id, username);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProject(UpdateProjectDto updateProjectDto)
    {
        await service.UpdateProject(updateProjectDto);
        return Ok();
    }

    [HttpGet("{userId:guid}/user-tasks")]
    public async Task<IActionResult> GetProjectsWithUserTasks(Guid userId)
    {
        return Ok(await service.GetProjectsWithUserTasks(userId));
    }
}
