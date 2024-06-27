using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Entities;
using Rindo.Infrastructure.Models;

namespace Rindo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly RindoDbContext _context;
        private readonly IProjectService _projectService;
        public InvitationController(RindoDbContext context, IProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvitation(Guid projectId, Guid userId)
        {
            var inv = new Invitation() { ProjectId = projectId, UserId = userId };
            _context.Invitations.Add(inv);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetInvitationsByUserId(Guid userId)
        {
            var invites = await _context.Invitations.Where(inv => inv.UserId == userId).ToListAsync();
            return Ok(invites);
        }

        [HttpGet("project")]
        public async Task<IActionResult> GetInvitationsByProjectId(Guid projectId)
        {
            var invites = await _context.Invitations.Where(inv => inv.ProjectId == projectId).ToListAsync();
            return Ok(invites);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteInvitation(Guid id)
        {
            var inv = await _context.Invitations.FirstOrDefaultAsync(inv => inv.Id == id);
            _context.Invitations.Remove(inv);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("{id:guid}")]
        public async Task<IActionResult> AcceptInvitation(Guid id)
        {
            var invitation = await _context.Invitations.FirstOrDefaultAsync(inv => inv.Id == id);
            var result = await _projectService.AddUserToProject(invitation.ProjectId, invitation.UserId);
            if (!result.IsSuccess) return BadRequest(result.Error.Description);
            return Ok();
        }
    }
}
