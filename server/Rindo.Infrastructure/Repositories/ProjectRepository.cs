using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
{
    public ProjectRepository(PostgresDbContext context) : base(context)
    {
    }
    
    public Task CreateProject(Project project) => CreateAsync(project);

    public void DeleteProject(Project project) => Delete(project);

    public void UpdateProject(Project project) => Update(project);

    public async Task<Project?> GetProjectById(Guid id) => 
        await FindByCondition(p => p.Id == id)
            .Include(p => p.Users)
            .Include(p => p.Roles)
            .Include(p => p.Stages.OrderBy(s => s.Index))
            .ThenInclude(p => p.Tasks.OrderBy(t => t.Index))
            .ThenInclude(t => t.Comments)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Project>> GetProjectsByUserId(Guid userId) =>
        await FindByCondition(p => p.OwnerId == userId).ToListAsync();
    
    public async Task UpdateProperty<TProperty>(Project project, Expression<Func<Project, TProperty>> expression) =>
        await UpdatePropertyAsync<TProperty>(project, expression);

    public async Task<IEnumerable<Project>> GetProjectsWhereUserAttends(User user) =>
        await FindByCondition(p => p.Users.Contains(user)).ToListAsync();
}