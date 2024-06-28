using Rindo.Domain.Common;
using Rindo.Domain.Entities;
using Task = System.Threading.Tasks.Task;
namespace Application.Interfaces.Services;

public interface IInvitationService
{
    Task CreateInvitation(Guid projectId, Guid userId);
    Task<Result> DeleteInvitation(Guid id);
    Task<Result> AcceptInvitation(Guid id);
    Task<IEnumerable<Invitation>> GetInvitationsByProjectId(Guid projectId);
    Task<IEnumerable<Invitation>> GetInvitationsByUserId(Guid userId);
}