using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class UserProjectRoleRepository : RepositoryBase<UserProjectRole>, IUserProjectRoleRepository
{
    public UserProjectRoleRepository(RindoDbContext context) : base(context)
    {
    }

    public Task CreateRelation(UserProjectRole relation) => CreateAsync(relation);

    public Task DeleteRelation(UserProjectRole relation) => DeleteAsync(relation);

    public async Task<IEnumerable<UserProjectRole>> GetRelationsByUserId(Guid projectId, Guid userId) =>
        await FindByCondition(u => u.ProjectId == projectId && u.UserId == userId)
            .Include(r => r.Role)
            .ToListAsync();
    public async Task<UserProjectRole?> GetRelationByIds(Guid projectId, Guid roleId, Guid userId) => 
        await FindByCondition(u => u.RoleId == roleId && u.ProjectId == projectId && u.UserId == userId)
            .Include(u => u.User)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<UserProjectRole>> GetRelationsByProjectId(Guid projectId, Guid roleId) => 
        await FindByCondition(u => u.RoleId == roleId && u.ProjectId == projectId)
            .Include(u => u.User)
            .ToListAsync();
}