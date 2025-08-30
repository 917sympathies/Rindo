using System.IdentityModel.Tokens.Jwt;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProjectController(IProjectService service, IHttpContextAccessor httpContextAccessor) : ControllerBase
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

    // [ServiceFilter(typeof(AsyncActionAccessFilter))]
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

    // [ServiceFilter(typeof(AsyncActionAccessFilter))]
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
        var token = httpContextAccessor.HttpContext?.Request.Cookies["_rindo"];
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var userId = jwtSecurityToken.Claims.First(c => c.Type == "userId").Value;
        await service.InviteUserToProject(id, username, Guid.Parse(userId));
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
    
    [HttpPut("{projectId:guid}/name")]
    public async Task<IActionResult> UpdateProjectName(Guid projectId, string name)
    {
        await service.UpdateProjectName(projectId, name);
        return Ok();
    }
    
    [HttpPut("{projectId:guid}/desc")]
    public async Task<IActionResult> UpdateProjectDescription(Guid projectId, string description)
    {
        await service.UpdateProjectDescription(projectId, description);
        return Ok();
    }

    [HttpPut("{projectId:guid}/stages")]
    public async Task<IActionResult> UpdateProjectStages(Guid projectId,[FromBody] Stage[] stages)
    {
        await service.UpdateProjectStages(projectId, stages);
        return Ok();
    }

    [HttpGet("{userId:guid}/userTasks")]
    public async Task<IActionResult> GetProjectsWithUserTasks(Guid userId)
    {
        return Ok(await service.GetProjectsWithUserTasks(userId));
    }
}
