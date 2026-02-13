using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ChatController(IChatService service) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetChatById(Guid id)
    {
        return Ok(await service.GetChatById(id));
    }
}
