using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
    public RoleRepository(PostgresDbContext context) : base(context)
    {
    }
    
    public Task CreateRole(Role role) => CreateAsync(role);

    public void DeleteRole(Role role) => Delete(role);

    public void UpdateRole(Role role) => Update(role);
    
    public async Task UpdateProperty<TProperty>(Role role, Expression<Func<Role, TProperty>> expression) =>
        await UpdatePropertyAsync<TProperty>(role, expression);

    public async Task<Role?> GetRoleById(Guid id) =>
        await FindByCondition(r => r.Id == id)
            .Include(r => r.Users)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Role>> GetRolesByProjectId(Guid projectId) => 
        await FindByCondition(r => r.ProjectId == projectId)
            .Include(r => r.Users)
            .ToListAsync();
}