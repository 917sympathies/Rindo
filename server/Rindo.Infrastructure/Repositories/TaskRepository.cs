using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class TaskRepository : RepositoryBase<Domain.Entities.Task>, ITaskRepository
{
    public TaskRepository(RindoDbContext context) : base(context)
    {
    }

    public Task CreateTask(Domain.Entities.Task task) => CreateAsync(task);

    public Task DeleteTask(Domain.Entities.Task task) => DeleteAsync(task);

    public Task UpdateTask(Domain.Entities.Task task) => UpdateAsync(task);

    public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByProjectId(Guid projectId) =>
        await FindByCondition(t => t.ProjectId == projectId).AsNoTracking().OrderBy(t => t.StartDate).ToListAsync();

    public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByUserId(Guid userId) =>
        await FindByCondition(t => t.ResponsibleUserId == userId).AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByStageId(Guid processId) =>
        await FindByCondition(t => t.StageId == processId).AsNoTracking().ToListAsync();

    public async Task<Domain.Entities.Task?> GetById(Guid id) =>
        await FindByCondition(t => t.Id == id)
            .Include(t => t.Comments)
            .AsNoTracking()
            .FirstOrDefaultAsync();

    public async Task UpdateProperty<TProperty>(Domain.Entities.Task task, Expression<Func<Domain.Entities.Task, TProperty>> expression) =>
        await UpdatePropertyAsync<TProperty>(task, expression);
}