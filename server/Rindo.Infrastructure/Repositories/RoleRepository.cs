using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Models;

using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class RoleRepository(PostgresDbContext context) : RepositoryBase<Role>(context), IRoleRepository
{
    private readonly PostgresDbContext _context = context;
    public Task CreateRole(Role role) => CreateAsync(role);

    public void DeleteRole(Role role) => Delete(role);

    public void UpdateRole(Role role) => Update(role);
    public async Task AddUserToRole(Guid roleId, Guid userId)
    {
        var result = await _context.Database.ExecuteSqlAsync($"INSERT INTO dbo.Roles2Users VALUES({roleId}, {userId})");
    }
    
    public async Task RemoveUserFromRole(Guid roleId, Guid userId)
    {
        var result = await _context.Database.ExecuteSqlAsync($"DELETE FROM dbo.Roles2Users WHERE RoleId = {roleId} AND UserId = {userId}");
    }

    public async Task<Role[]> GetRolesByUserId(Guid userId)
    {
        var result = await _context.Roles.Where(r => r.Users.Contains(new User { Id = userId })).ToArrayAsync();
        return result;
    }

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