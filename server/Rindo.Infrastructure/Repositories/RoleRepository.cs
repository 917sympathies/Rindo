using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
    public RoleRepository(RindoDbContext context) : base(context)
    {
    }
    
    public Task CreateRole(Role role) => CreateAsync(role);

    public Task DeleteRole(Role role) => DeleteAsync(role);

    public Task UpdateRole(Role role) => UpdateAsync(role);
    
    public async Task UpdateProperty<TProperty>(Role role, Expression<Func<Role, TProperty>> expression) =>
        await UpdatePropertyAsync<TProperty>(role, expression);

    public async Task<Role?> GetRoleById(Guid id) =>
        await FindByCondition(r => r.Id == id)
            .Include(r => r.UserProjectRoles)
            .ThenInclude(u => u.User)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Role>> GetRolesByProjectId(Guid projectId) => 
        await FindByCondition(r => r.ProjectId == projectId)
            .Include(r => r.UserProjectRoles)
            .ThenInclude(u => u.User)
            .ToListAsync();
}