using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.Models;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class InvitationService : IInvitationService
{
    private readonly RindoDbContext _context;
    
    private readonly IProjectService _projectService;

    public InvitationService(RindoDbContext context, IProjectService projectService)
    {
        _context = context;
        _projectService = projectService;
    }

    public async Task CreateInvitation(Guid projectId, Guid userId)
    {
        var inv = new Invitation { ProjectId = projectId, UserId = userId };
        _context.Invitations.Add(inv);
        await _context.SaveChangesAsync();
    }

    public async Task<Result> DeleteInvitation(Guid id)
    {
        var invite = await _context.Invitations.FirstOrDefaultAsync(inv => inv.Id == id);
        if (invite == null) return Error.NotFound("Invitation with this id doesn't exists");
        _context.Invitations.Remove(invite);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AcceptInvitation(Guid id)
    {
        var invitation = await _context.Invitations.FirstOrDefaultAsync(inv => inv.Id == id);
        if (invitation is null) return Error.NotFound("Invitation with this id doesn't exists");
        var result = await _projectService.AddUserToProject(invitation.ProjectId, invitation.UserId);
        return !result.IsSuccess ? result.Error : result;
    }

    public async Task<IEnumerable<object>> GetInvitationsByProjectId(Guid projectId)
    {
        var invites = await _context.Invitations.Where(inv => inv.ProjectId == projectId).ToListAsync();
        var result = invites.Select(inv => new { id = inv.Id, sender = inv.SenderUsername , user = _context.Users.FirstOrDefault(u => u.Id == inv.UserId)?.Username });
        return result;
    }

    public async Task<IEnumerable<Invitation>> GetInvitationsByUserId(Guid userId)
    {
        var invites = await _context.Invitations.Where(inv => inv.UserId == userId).ToListAsync();
        return invites;
    }
}