using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Application.Services.StageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;

namespace Rindo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StageController : ControllerBase
    {
        private readonly IStageService _service;
        public StageController(IStageService service)
        {
            _service = service;
        }

        [HttpGet("{id:guid}/name")]
        public async Task<IActionResult> GetStageName(Guid id)
        {
            var result = await _service.GetStageName(id);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetStagesByProject(Guid projectId)
        {
            var result = await _service.GetStagesByProjectId(projectId);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> ChangeStageTask(Guid id, Guid taskId)
        {
            var result = await _service.ChangeStageTask(id, taskId);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddStage([FromBody]StageOnCreateDto stageDto)
        {
            await _service.AddStage(stageDto);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteStage(Guid id)
        {
            var result = await _service.DeleteStage(id);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok();
        }
    }
}
