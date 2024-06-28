using Application.Interfaces.Services;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.Entities;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Services;

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
        var inv = new Invitation() { ProjectId = projectId, UserId = userId };
        _context.Invitations.Add(inv);
        await _context.SaveChangesAsync();
    }

    public async Task<Result> DeleteInvitation(Guid id)
    {
        var invite = await _context.Invitations.FirstOrDefaultAsync(inv => inv.Id == id);
        if (invite == null) return Error.NotFound("Такого приглашения не существует!");
        _context.Invitations.Remove(invite);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AcceptInvitation(Guid id)
    {
        var invitation = await _context.Invitations.FirstOrDefaultAsync(inv => inv.Id == id);
        if (invitation is null) return Error.NotFound("Такого приглашения не существует");
        var result = await _projectService.AddUserToProject(invitation.ProjectId, invitation.UserId);
        return !result.IsSuccess ? result.Error : result;
    }

    public async Task<IEnumerable<Invitation>> GetInvitationsByProjectId(Guid projectId)
    {
        var invites = await _context.Invitations.Where(inv => inv.ProjectId == projectId).ToListAsync();
        return invites;
    }

    public async Task<IEnumerable<Invitation>> GetInvitationsByUserId(Guid userId)
    {
        var invites = await _context.Invitations.Where(inv => inv.UserId == userId).ToListAsync();
        return invites;
    }
}