using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Models;

namespace Rindo.Infrastructure.Repositories;

public class TaskRepository(PostgresDbContext context) : RepositoryBase<ProjectTask>(context), ITaskRepository
{
    private readonly PostgresDbContext _context = context;
    public Task<ProjectTask> CreateTask(ProjectTask projectTask) => CreateAsync(projectTask);

    public void DeleteTask(ProjectTask projectTask) => Delete(projectTask);

    public void UpdateTask(ProjectTask projectTask) => Update(projectTask);

    public async Task UnassignTasksFromUser(Guid projectId, Guid userId)
    {
        // TODO: maybe we shouldn't delete records but mark them as 'Deleted' with IsDeleted flag
        await _context.Database.ExecuteSqlAsync($"UPDATE dbo.Tasks SET AssigneeId = NULL WHERE AssigneeId = '{userId}'");
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByProjectId(Guid projectId) =>
        await FindByCondition(t => t.ProjectId == projectId).AsNoTracking().OrderBy(t => t.CreatedDate).ToListAsync();

    public async Task<IEnumerable<ProjectTask>> GetTasksInProjectAssignedToUser(Guid projectId, Guid userId) =>
        await FindByCondition(t => t.AssigneeId == userId && t.ProjectId == projectId).AsNoTracking().ToListAsync();
    
    public async Task<IEnumerable<ProjectTask>> GetTasksAssignedToUser(Guid userId) =>
        await FindByCondition(t => t.AssigneeId == userId).AsNoTracking().ToListAsync();

    public async Task<IEnumerable<ProjectTask>> GetTasksByStageId(Guid processId) =>
        await FindByCondition(t => t.StageId == processId).AsNoTracking().ToListAsync();

    public async Task<ProjectTask?> GetById(Guid id) =>
        await FindByCondition(t => t.Id == id)
            .Include(t => t.Comments)
            .AsNoTracking()
            .FirstOrDefaultAsync();

    public async Task UpdateProperty<TProperty>(ProjectTask projectTask, Expression<Func<ProjectTask, TProperty>> expression) =>
        await UpdatePropertyAsync<TProperty>(projectTask, expression);
}