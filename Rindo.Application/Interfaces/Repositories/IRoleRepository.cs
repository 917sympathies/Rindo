using System.Linq.Expressions;
using Rindo.Domain.DataObjects;

namespace Application.Interfaces.Repositories;

public interface IRoleRepository
{
    Task CreateRole(Role role);
    Task DeleteRole(Role role);
    Task UpdateProperty<TProperty>(Role role, Expression<Func<Role, TProperty>> expression);
    Task UpdateRole(Role role);
    Task AddUserToRole(Guid roleId, Guid userId);
    Task RemoveUserFromRole(Guid roleId, Guid userId);
    Task RemoveRolesByProjectId(Guid projectId);
    Task<Role[]> GetRolesByUserId(Guid userId);
    Task<Role?> GetRoleById(Guid id);
    Task<IEnumerable<Role>> GetRolesByProjectId(Guid projectId);
}