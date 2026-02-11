using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;

namespace Rindo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController(IUserService service) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        return Ok(await service.GetUserById(id));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsersByProjectId(Guid projectId)
    {
        return Ok(await service.GetUsersByProjectId(projectId));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(UserDto userDto)
    {
        await service.UpdateUser(userDto);
        return Ok();
    }
}
