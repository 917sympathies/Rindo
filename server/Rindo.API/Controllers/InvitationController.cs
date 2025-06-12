using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.Common;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InvitationController : ControllerBase
{
    private readonly IInvitationService _service;
    
    public InvitationController(IInvitationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvitation(Guid projectId, Guid userId)
    {
        await _service.CreateInvitation(projectId, userId);
        return Ok();
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetInvitationsByUserId(Guid userId)
    {
        var invites = await _service.GetInvitationsByUserId(userId);
        return Ok(invites);
    }

    [HttpGet("project")]
    public async Task<IActionResult> GetInvitationsByProjectId(Guid projectId)
    {
        var invites = await _service.GetInvitationsByProjectId(projectId);
        return Ok(invites);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteInvitation(Guid id)
    {
        var result = await _service.DeleteInvitation(id);
        if (!result.IsSuccess) return NotFound(Error.NotFound(result.Error.Description));
        return Ok();
    }

    [HttpPost("{id:guid}")]
    public async Task<IActionResult> AcceptInvitation(Guid id)
    {
        var result = await _service.AcceptInvitation(id);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok();
    }
}
