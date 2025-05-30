using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;
namespace Application.Interfaces.Services;

public interface IInvitationService
{
    Task CreateInvitation(Guid projectId, Guid userId);
    Task<Result> DeleteInvitation(Guid id);
    Task<Result> AcceptInvitation(Guid id);
    Task<IEnumerable<InviteDto>> GetInvitationsByProjectId(Guid projectId);
    Task<IEnumerable<Invitation>> GetInvitationsByUserId(Guid userId);
}