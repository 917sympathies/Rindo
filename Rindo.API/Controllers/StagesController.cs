using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Roles;
using Rindo.Domain.DTO.Tasks;

namespace Rindo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class StagesController(IStageService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStagesByProject(Guid projectId)
    {
        var result = await service.GetStagesByProjectId(projectId);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddStage([FromBody]StageOnCreateDto stageDto)
    {
        await service.AddStage(stageDto);
        return Ok();
    }

    [HttpDelete("{stageId:guid}")]
    public async Task<IActionResult> DeleteStage(Guid stageId)
    {
        await service.DeleteStage(stageId);
        return Ok();
    }
}
