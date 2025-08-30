using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Rindo.API.Controllers;
    
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CommentController(ICommentService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCommentsCountByTaskId(Guid taskId)
    {
        var result = await service.GetCommentsCountByTaskId(taskId);
        return Ok(result);
    }
}
