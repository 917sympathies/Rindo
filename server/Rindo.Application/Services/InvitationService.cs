using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;
using Rindo.Infrastructure;

namespace Application.Services;

public class InvitationService(
    IProjectService projectService,
    IInvitationRepository invitationRepository,
    IUserRepository userRepository) : IInvitationService
{
    public async Task CreateInvitation(Guid projectId, Guid userId)
    {
        var inv = new Invitation { ProjectId = projectId, RecipientId = userId };
        await invitationRepository.CreateInvitation(inv);
    }

    public async Task CreateInvitation(Invitation invitation)
    {
        await invitationRepository.CreateInvitation(invitation);
    }

    public async Task DeleteInvitation(Guid invitationId)
    {
        var invitation = await invitationRepository.GetInvitationById(invitationId);
        if (invitation == null) throw new NotFoundException(nameof(Invitation), invitationId);
        await invitationRepository.DeleteInvitation(invitation);
    }

    public async Task AcceptInvitation(Guid invitationId)
    {
        var invitation = await invitationRepository.GetInvitationById(invitationId);
        if (invitation is null) throw new NotFoundException(nameof(Invitation), invitationId);
        await projectService.AddUserToProject(invitation.ProjectId, invitation.RecipientId);
    }

    public async Task<IEnumerable<InviteDto>> GetInvitationsByProjectId(Guid projectId)
    {
        var invites = await invitationRepository.GetInvitationsByProjectId(projectId);
        var sendersIds = invites.Select(x => x.SenderId);
        var recipientsIds = invites.Select(x => x.RecipientId);
        var senders = await userRepository.GetUsersByIds(sendersIds.ToArray());
        var recipients = await userRepository.GetUsersByIds(recipientsIds.ToArray());
        
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
        return await invitationRepository.GetInvitationsByUserId(userId);
    }
}