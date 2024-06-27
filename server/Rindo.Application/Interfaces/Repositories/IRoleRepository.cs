using System.Linq.Expressions;
using Rindo.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface IRoleRepository
{
    Task CreateRole(Role role);
    Task DeleteRole(Role role);
    Task UpdateProperty<TProperty>(Role role, Expression<Func<Role, TProperty>> expression);
    Task UpdateRole(Role role);
    Task<Role?> GetRoleById(Guid id);
    Task<IEnumerable<Role>> GetRolesByProjectId(Guid projectId);
}