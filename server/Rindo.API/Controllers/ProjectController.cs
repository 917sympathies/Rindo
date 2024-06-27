using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RindoDbContext _context;
        public ProjectController(IProjectService service, RindoDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _context = context;
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
            var result = await _service.InviteUserToProject(id, username, user.Username);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok();
        }
        
        [HttpPost("{id:guid}")]
        public async Task<IActionResult> AddUserToProject(Guid id, Guid userId)
        {
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            //await _service.AddUserToProject(id, userId);
            //if (!result.IsSuccess) return BadRequest(result.Error);
            var invitation =
                await _context.Invitations.FirstOrDefaultAsync(inv =>
                    inv.ProjectId == id && userId == inv.UserId);
            _context.Invitations.Remove(invitation);
            await _context.SaveChangesAsync();
            var project = await _context.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == id);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            project.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("{id:guid}/remove")]
        public async Task<IActionResult> RemoveUserFromProject(Guid id, string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user is null) return NotFound("Такого пользователя не существует!");
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            await _context.Entry(project).Collection(p => p.Users).LoadAsync();
            project.Users.Remove(user);
            
            var upr = await _context.UserProjectRoles.Where(up => up.UserId == user.Id && up.ProjectId == id).ToListAsync();
            _context.UserProjectRoles.RemoveRange(upr);

            var tasks = await _context.Tasks.Where(t => t.ResponsibleUserId == user.Id && t.ProjectId == id).ToListAsync();
            foreach (var task in tasks) task.ResponsibleUserId = null;
            
            await _context.SaveChangesAsync();
            
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
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            project.Stages = stages;
            await _context.SaveChangesAsync();           
            return Ok();
        }

        [HttpGet("{userId:guid}/usertasks")]
        public async Task<IActionResult> GetProjectsWithUserTasks(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var tasks = await _context.Tasks.Where(t => t.ResponsibleUserId == userId).ToListAsync();
            var projects = await _context.Projects.Where(p => p.Users.Contains(user) || p.OwnerId == user.Id).ToListAsync();
            var result = projects.Select(p => new {p.Name, p.Id, tasks = tasks.Where(t => t.ProjectId == p.Id) }).ToList();
            return Ok(result);
        }
    }
}
