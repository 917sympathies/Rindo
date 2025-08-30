using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.Common;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InvitationController(IInvitationService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateInvitation(Guid projectId, Guid userId)
    {
        await service.CreateInvitation(projectId, userId);
        return Ok();
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetInvitationsByUserId(Guid userId)
    {
        var invites = await service.GetInvitationsByUserId(userId);
        return Ok(invites);
    }

    [HttpGet("project")]
    public async Task<IActionResult> GetInvitationsByProjectId(Guid projectId)
    {
        var invites = await service.GetInvitationsByProjectId(projectId);
        return Ok(invites);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteInvitation(Guid id)
    {
        await service.DeleteInvitation(id);
        return Ok();
    }

    [HttpPost("{id:guid}")]
    public async Task<IActionResult> AcceptInvitation(Guid id)
    {
        await service.AcceptInvitation(id);
        return Ok();
    }
}
