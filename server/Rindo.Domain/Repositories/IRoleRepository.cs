using System.Linq.Expressions;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface IRoleRepository
{
    Task CreateRole(Role role);
    void DeleteRole(Role role);
    Task UpdateProperty<TProperty>(Role role, Expression<Func<Role, TProperty>> expression);
    void UpdateRole(Role role);
    Task<Role?> GetRoleById(Guid id);
    Task<IEnumerable<Role>> GetRolesByProjectId(Guid projectId);
}