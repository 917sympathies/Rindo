using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class ProjectRepository(PostgresDbContext context) : RepositoryBase<Project>(context), IProjectRepository
{
    private readonly PostgresDbContext _context = context;
    public Task<Project> CreateProject(Project project) => CreateAsync(project);

    public void DeleteProject(Project project) => Delete(project);

    public void UpdateProject(Project project) => Update(project);

    public async Task<Project?> GetProjectById(Guid id) => 
        await FindByCondition(p => p.Id == id)
            .Include(p => p.Users)
            .Include(p => p.Roles)
            .Include(p => p.Stages.OrderBy(s => s.Index))
            .ThenInclude(p => p.Tasks.OrderBy(t => t.Index))
            //.ThenInclude(t => t.Comments)
            .FirstOrDefaultAsync();
    
    public async Task<ProjectHeaderInfoDto?> GetProjectHeaderInfo(Guid projectId) =>
        await _context.Projects
            .Where(x => x.Id == projectId)
            .Select(x => new ProjectHeaderInfoDto
            {
                Name = x.Name,
                OwnerId = x.OwnerId,
                ChatId = x.ChatId,
            })
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Project>> GetProjectsOwnedByUser(Guid userId) =>
        await FindByCondition(p => p.OwnerId == userId).ToListAsync();

    public async Task<Project?> GetProjectByIdWithUsers(Guid projectId) =>
        await FindByCondition(p => p.Id == projectId)
            .Include(x => x.Users)
            .FirstOrDefaultAsync();

    public async Task UpdateProperty<TProperty>(Project project, Expression<Func<Project, TProperty>> expression) =>
        await UpdatePropertyAsync<TProperty>(project, expression);

    public async Task<IEnumerable<Project>> GetProjectsWhereUserAttends(Guid userId) =>
        await FindByCondition(p => p.Users.Any(x => x.Id == userId) || p.OwnerId == userId).ToListAsync();
}