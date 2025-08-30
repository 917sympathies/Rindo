using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TagsController(ITagService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTag(string name, Guid projectId)
    {
        var tag = await service.CreateTag(name, projectId);
        return Ok(tag);
    }

    [HttpGet]
    public async Task<IActionResult> GetTagsByProjectId(Guid projectId)
    {
        var tags = await service.GetTagsByProjectId(projectId);
        return Ok(tags);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        await service.DeleteTag(id);
        return Ok();
    }
}
