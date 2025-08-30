using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Rindo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserService service) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await service.GetUserById(id);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsersByProjectId(Guid projectId)
    {
        var result = await service.GetUsersByProjectId(projectId);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/firstName")]
    public async Task<IActionResult> ChangeUserFirstName(Guid id, string firstName)
    {
        var result = await service.ChangeUserFirstName(id, firstName);
        if (!result.IsSuccess) return NotFound(result.Error.Description); 
        return Ok();
    }
    
    [HttpPut("{id:guid}/lastName")]
    public async Task<IActionResult> ChangeUserLastName(Guid id, string lastName)
    {
        var result = await service.ChangeUserLastName(id, lastName);
        if (!result.IsSuccess) return NotFound(result.Error.Description);
        return Ok();
    }
}
