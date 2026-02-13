using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Roles;
using Rindo.Domain.Enums;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleDtoOnCreate roleDto)
    {
        await service.CreateRole(roleDto);
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRolesByProjectId(Guid projectId)
    {
        var result = await service.GetRolesByProjectId(projectId);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody]Permissions rights)
    {
        await service.UpdateRoleRights(id, rights);
        return Ok();
    }

    [HttpPut("{id:guid}/name")]
    public async Task<IActionResult> UpdateRoleName(Guid id, string name)
    {
        await service.UpdateRoleName(id, name);
        return Ok();
    }

    [HttpGet("{projectId:guid}/{userId:guid}")]
    public async Task<IActionResult> GetRightsForUserByProject(Guid projectId, Guid userId)
    {
        var result = await service.GetRightsByProjectId(projectId, userId);
        return Ok(result);
    }

    [HttpPut("{id:guid}/add-user")]
    public async Task<IActionResult> AddUserToRole(Guid id, Guid userId)
    {
        await service.AddUserToRole(id, userId);
        return Ok();
    }

    [HttpPut("{id:guid}/remove-user")]
    public async Task<IActionResult> RemoveUserFromRole(Guid id, Guid userId)
    {
        await service.RemoveUserFromRole(id, userId);
        return Ok();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        await service.DeleteRole(id);
        return Ok();
    }
}
