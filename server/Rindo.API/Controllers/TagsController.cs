using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly ITagService _service;
    
    public TagsController(ITagService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag(string name, Guid projectId)
    {
        var tag = await _service.CreateTag(name, projectId);
        return Ok(tag);
    }

    [HttpGet]
    public async Task<IActionResult> GetTagsByProjectId(Guid projectId)
    {
        var tags = await _service.GetTagsByProjectId(projectId);
        return Ok(tags);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        await _service.DeleteTag(id);
        return Ok();
    }
}
