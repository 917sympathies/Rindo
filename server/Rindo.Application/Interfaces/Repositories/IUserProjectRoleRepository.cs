using Rindo.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface IUserProjectRoleRepository
{
    Task CreateRelation(UserProjectRole relation);
    Task DeleteRelation(UserProjectRole relation);
    Task<IEnumerable<UserProjectRole>> GetRelationsByUserId(Guid projectId, Guid userId);
    Task<UserProjectRole?> GetRelationByIds(Guid projectId, Guid roleId, Guid userId);
    Task<IEnumerable<UserProjectRole>> GetRelationsByProjectId(Guid projectId, Guid roleId);
}