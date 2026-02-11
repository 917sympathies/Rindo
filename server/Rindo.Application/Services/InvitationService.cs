using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;
using Rindo.Domain.DataObjects;

namespace Application.Services;

public class InvitationService(
    IProjectRepository projectRepository,
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
        await projectRepository.AddUserToProject(invitation.ProjectId, invitation.RecipientId);
    }

    public async Task<IEnumerable<InvitationProjectInfoDto>> GetInvitationsByProjectId(Guid projectId)
    {
        var invites = await invitationRepository.GetInvitationsByProjectId(projectId);
        var sendersIds = invites.Select(x => x.SenderId);
        var recipientsIds = invites.Select(x => x.RecipientId);
        var senders = await userRepository.GetUsersByIds(sendersIds.ToArray());
        var recipients = await userRepository.GetUsersByIds(recipientsIds.ToArray());
        
        var result = invites.Select(inv => new InvitationProjectInfoDto
        { 
            Id = inv.Id, 
            SenderUsername = senders.FirstOrDefault(x => x.UserId == inv.SenderId)?.Username ?? string.Empty, 
            RecipientUsername = recipients.FirstOrDefault(x => x.UserId == inv.RecipientId)?.Username ?? string.Empty,
        });
        return result;
    }

    public async Task<IEnumerable<InvitationDto>> GetInvitationsByUserId(Guid userId)
    {
        var invitations = await invitationRepository.GetInvitationsByUserId(userId);
        var senders = await userRepository.GetUsersByIds(invitations.Select(x => x.SenderId).ToArray());
        var projects = await projectRepository.GetProjectsByIds(invitations.Select(x => x.ProjectId).ToArray());
        return invitations.Select(inv => new InvitationDto
        {
            InvitationId = inv.Id,
            ProjectId = inv.ProjectId,
            SenderId = inv.SenderId,
            ProjectName = projects.FirstOrDefault(x => x.ProjectId == inv.ProjectId)?.Name ?? string.Empty,
            SenderUsername = senders.FirstOrDefault(x => x.UserId == inv.SenderId)?.Username ?? string.Empty,
        });
    }
}