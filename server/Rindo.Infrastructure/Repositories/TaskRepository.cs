using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class TaskRepository : RepositoryBase<Domain.Models.ProjectTask>, ITaskRepository
{
    public TaskRepository(RindoDbContext context) : base(context)
    {
    }

    public Task CreateTask(Domain.Models.ProjectTask projectTask) => CreateAsync(projectTask);

    public Task DeleteTask(Domain.Models.ProjectTask projectTask) => DeleteAsync(projectTask);

    public Task UpdateTask(Domain.Models.ProjectTask projectTask) => UpdateAsync(projectTask);

    public async Task<IEnumerable<Domain.Models.ProjectTask>> GetTasksByProjectId(Guid projectId) =>
        await FindByCondition(t => t.ProjectId == projectId).AsNoTracking().OrderBy(t => t.StartDate).ToListAsync();

    public async Task<IEnumerable<Domain.Models.ProjectTask>> GetTasksByUserId(Guid userId) =>
        await FindByCondition(t => t.ResponsibleUserId == userId).AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Domain.Models.ProjectTask>> GetTasksByStageId(Guid processId) =>
        await FindByCondition(t => t.StageId == processId).AsNoTracking().ToListAsync();

    public async Task<Domain.Models.ProjectTask?> GetById(Guid id) =>
        await FindByCondition(t => t.Id == id)
            .Include(t => t.Comments)
            .AsNoTracking()
            .FirstOrDefaultAsync();

    public async Task UpdateProperty<TProperty>(Domain.Models.ProjectTask projectTask, Expression<Func<Domain.Models.ProjectTask, TProperty>> expression) =>
        await UpdatePropertyAsync<TProperty>(projectTask, expression);
}