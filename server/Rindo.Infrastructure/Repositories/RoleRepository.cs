using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.DataObjects;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class RoleRepository(PostgresDbContext context) : RepositoryBase<Role>(context), IRoleRepository
{
    private readonly PostgresDbContext _context = context;
    
    public Task CreateRole(Role role) => CreateAsync(role);
    
    public async Task UpdateRole(Role role) => await Update(role);
    
    public async Task DeleteRole(Role role)=> await Delete(role);
    
    public async Task AddUserToRole(Guid roleId, Guid userId)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO dbo.roles_to_users VALUES({roleId}, {userId})");
    }
    
    public async Task RemoveUserFromRole(Guid roleId, Guid userId)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM dbo.roles_to_users WHERE RoleId = {roleId} AND UserId = {userId}");
    }

    public async Task RemoveRolesByProjectId(Guid projectId)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM dbo.Roles WHERE ProjectId = {projectId}");
    }

    public async Task<Role[]> GetRolesByUserId(Guid userId)
    {
        var result = await _context.Roles.Where(r => r.Users.Contains(new User { UserId = userId })).ToArrayAsync();
        return result;
    }

    public async Task UpdateProperty<TProperty>(Role role, Expression<Func<Role, TProperty>> expression) =>
        await UpdatePropertyAsync<TProperty>(role, expression);

    public async Task<Role?> GetRoleById(Guid id) =>
        await FindByCondition(r => r.RoleId == id)
            .Include(r => r.Users)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Role>> GetRolesByProjectId(Guid projectId) => 
        await FindByCondition(r => r.ProjectId == projectId)
            .Include(r => r.Users)
            .ToListAsync();
}