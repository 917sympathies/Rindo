using System.IdentityModel.Tokens.Jwt;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.API.Filters;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;

namespace Rindo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProjectController(IProjectService service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectOnCreateDto projectOnCreateDto)
        {
            var project = await _service.CreateProject(projectOnCreateDto);
            return Ok(project);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var project = await _service.GetProjectById(id);
            return Ok(project);
        }

        [ServiceFilter(typeof(AsyncActionAccessFilter))]
        [HttpGet("{id:guid}/settings")]
        public async Task<IActionResult> GetProjectSettings(Guid id)
        {
            var project = await _service.GetProjectSettings(id);
            return Ok(project);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectsWhereUserAttends(Guid userId)
        {
            return Ok(await _service.GetProjectsWhereUserAttends(userId));
        }

        [ServiceFilter(typeof(AsyncActionAccessFilter))]
        [HttpGet("{id:guid}/header")]
        public async Task<IActionResult> GetProjectsInfoForHeader(Guid id)
        {
            return Ok(await _service.GetProjectsInfoForHeader(id));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var result = await _service.DeleteProject(id);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok();
        }

        [HttpPost("{id:guid}/invite")]
        public async Task<IActionResult> InviteUserToProject(Guid id, string username)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["test-cookies"];
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var userId = jwtSecurityToken.Claims.First(c => c.Type == "userId").Value;
            // var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
            var result = await _service.InviteUserToProject(id, username, Guid.Parse(userId));
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok();
        }
        
        [HttpPost("{id:guid}")]
        public async Task<IActionResult> AddUserToProject(Guid id, Guid userId)
        {
            var result = await _service.AddUserToProject(id, userId);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok();
        }

        [HttpPost("{id:guid}/remove")]
        public async Task<IActionResult> RemoveUserFromProject(Guid id, string username)
        {
            var result = await _service.RemoveUserFromProject(id, username);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok();
        }
        
        [HttpPut("{projectId:guid}/name")]
        public async Task<IActionResult> UpdateProjectName(Guid projectId, string name)
        {
            var result = await _service.UpdateProjectName(projectId, name);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok();
        }
        
        [HttpPut("{projectId:guid}/desc")]
        public async Task<IActionResult> UpdateProjectDescription(Guid projectId, string description)
        {
            var result = await _service.UpdateProjectDescription(projectId, description);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok();
        }

        [HttpPut("{projectId:guid}/stages")]
        public async Task<IActionResult> UpdateProjectStages(Guid projectId,[FromBody] Stage[] stages)
        {
            var result = await _service.UpdateProjectStages(projectId, stages);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok();
        }

        [HttpGet("{userId:guid}/userTasks")]
        public async Task<IActionResult> GetProjectsWithUserTasks(Guid userId)
        {
            var result = await _service.GetProjectsWithUserTasks(userId);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok(result.Value);
        }
    }
}
