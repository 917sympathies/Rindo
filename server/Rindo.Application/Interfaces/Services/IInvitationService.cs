using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;
namespace Application.Interfaces.Services;

public interface IInvitationService
{
    Task CreateInvitation(Guid projectId, Guid userId);
    Task CreateInvitation(Invitation invitation);
    Task DeleteInvitation(Guid invitationId);
    Task AcceptInvitation(Guid invitationId);
    Task<IEnumerable<InviteDto>> GetInvitationsByProjectId(Guid projectId);
    Task<IEnumerable<Invitation>> GetInvitationsByUserId(Guid userId);
}