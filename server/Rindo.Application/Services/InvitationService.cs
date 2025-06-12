using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;
using Rindo.Infrastructure;

namespace Application.Services;

public class InvitationService : IInvitationService
{
    private readonly PostgresDbContext _context; //TODO: remove DbContext
    
    private readonly IProjectService _projectService;

    public InvitationService(PostgresDbContext context, IProjectService projectService)
    {
        _context = context;
        _projectService = projectService;
    }

    public async Task CreateInvitation(Guid projectId, Guid userId)
    {
        var inv = new Invitation { ProjectId = projectId, RecipientId = userId };
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
        var result = await _projectService.AddUserToProject(invitation.ProjectId, invitation.RecipientId);
        return !result.IsSuccess ? result.Error : result;
    }

    public async Task<IEnumerable<InviteDto>> GetInvitationsByProjectId(Guid projectId)
    {
        var invites = await _context.Invitations.Where(inv => inv.ProjectId == projectId).ToListAsync();
        var sendersIds = invites.Select(x => x.SenderId);
        var recipientsIds = invites.Select(x => x.RecipientId);
        var sendersTask = _context.Users.Where(x => sendersIds.Contains(x.Id)).ToListAsync();
        var recipientsTask = _context.Users.Where(x => recipientsIds.Contains(x.Id)).ToListAsync();
        await Task.WhenAll(sendersTask, recipientsTask);
        var senders = sendersTask.Result;
        var recipients = recipientsTask.Result;
        var result = invites.Select(inv => new InviteDto
        { 
            Id = inv.Id, 
            SenderUsername = senders.FirstOrDefault(x => x.Id == inv.SenderId)?.Username ?? string.Empty, 
            RecipientUsername = recipients.FirstOrDefault(x => x.Id == inv.RecipientId)?.Username ?? string.Empty,
        });
        return result;
    }

    public async Task<IEnumerable<Invitation>> GetInvitationsByUserId(Guid userId)
    {
        var invites = await _context.Invitations.Where(inv => inv.RecipientId == userId).ToListAsync();
        return invites;
    }
}