using Rindo.Domain.DataObjects;

namespace Application.Interfaces.Services;

public interface IInvitationRepository
{
    Task<Invitation> CreateInvitation(Invitation invitation);
    Task<Invitation?> GetInvitationById(Guid invitationId);
    Task<Invitation[]> GetInvitationsByProjectId(Guid projectId);
    Task<Invitation[]> GetInvitationsByUserId(Guid userId);
    Task DeleteInvitation(Invitation invitation);
}