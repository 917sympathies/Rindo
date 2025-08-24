using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class TaskRepository : RepositoryBase<ProjectTask>, ITaskRepository
{
    public TaskRepository(PostgresDbContext context) : base(context)
    {
    }

    public Task CreateTask(ProjectTask projectTask) => CreateAsync(projectTask);

    public void DeleteTask(ProjectTask projectTask) => Delete(projectTask);

    public void UpdateTask(ProjectTask projectTask) => Update(projectTask);

    public async Task<IEnumerable<ProjectTask>> GetTasksByProjectId(Guid projectId) =>
        await FindByCondition(t => t.ProjectId == projectId).AsNoTracking().OrderBy(t => t.CreatedDate).ToListAsync();

    public async Task<IEnumerable<ProjectTask>> GetTasksByUserId(Guid userId) =>
        await FindByCondition(t => t.AsigneeId == userId).AsNoTracking().ToListAsync();

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