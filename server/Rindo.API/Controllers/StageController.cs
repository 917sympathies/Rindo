using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class StageController(IStageService service) : ControllerBase
{
    [HttpGet("{stageId:guid}/name")]
    public async Task<IActionResult> GetStageName(Guid stageId)
    {
        return Ok(await service.GetStageName(stageId));
    }

    // [ServiceFilter(typeof(AsyncActionAccessFilter))]
    [HttpGet]
    public async Task<IActionResult> GetStagesByProject(Guid projectId)
    {
        var result = await service.GetStagesByProjectId(projectId);
        return Ok(result);
    }

    [HttpPut("{stageId:guid}")]
    public async Task<IActionResult> ChangeStageTask(Guid stageId, Guid taskId)
    {
        await service.ChangeStageTask(stageId, taskId);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddStage([FromBody]StageOnCreateDto stageDto)
    {
        await service.AddStage(stageDto);
        return Ok();
    }

    [HttpDelete("{stageId:guid}")]
    public async Task<IActionResult> DeleteStage(Guid stageId, Guid projectId)
    {
        await service.DeleteStage(stageId, projectId);
        return Ok();
    }
}
