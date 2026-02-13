using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;
using Rindo.Domain.DataObjects;
namespace Application.Interfaces.Services;

public interface IInvitationService
{
    Task CreateInvitation(Guid projectId, Guid userId);
    Task CreateInvitation(Invitation invitation);
    Task DeleteInvitation(Guid invitationId);
    Task AcceptInvitation(Guid invitationId);
    Task<IEnumerable<InvitationProjectInfoDto>> GetInvitationsByProjectId(Guid projectId);
    Task<IEnumerable<InvitationDto>> GetInvitationsByUserId(Guid userId);
}