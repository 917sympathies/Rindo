using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
{
    public ProjectRepository(RindoDbContext context) : base(context)
    {
    }
    public Task CreateProject(Project project) => CreateAsync(project);

    public Task DeleteProject(Project project) => DeleteAsync(project);

    public Task UpdateProject(Project project) => UpdateAsync(project);

    public async Task<Project?> GetProjectById(Guid id) => 
        await FindByCondition(p => p.Id == id)
            .Include(p => p.Owner)
            .Include(p => p.Users)
            //.Include(p => p.Tasks)
            .Include(p => p.Roles)
            .Include(p => p.Chat)
            .ThenInclude(c => c.Messages)
            .Include(p => p.Stages.OrderBy(s => s.Index))
            .ThenInclude(p => p.Tasks.OrderBy(t => t.Index))
            .ThenInclude(t => t.Comments)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Project>> GetProjectsByUserId(Guid userId) =>
        await FindByCondition(p => p.OwnerId == userId).ToListAsync();
    public async Task UpdateProperty<TProperty>(Project project, Expression<Func<Project, TProperty>> expression) =>
        await UpdatePropertyAsync<TProperty>(project, expression);

    public async Task UpdateCollection<TProperty>(Project project,
        Expression<Func<Project, IEnumerable<TProperty>>> expression) where TProperty : class =>
        await UpdateCollectionAsync<TProperty>(project, expression);


    public async Task<IEnumerable<Project>> GetProjectsWhereUserAttends(User user) =>
        await FindByCondition(p => p.Users.Contains(user)).ToListAsync();
}