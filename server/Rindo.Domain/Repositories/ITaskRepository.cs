using System.Linq.Expressions;
using Rindo.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;
using ProjectTask = Domain.Entities.Task;

public interface ITaskRepository
{
    Task CreateTask(ProjectTask task);
    Task DeleteTask(ProjectTask task);
    Task UpdateTask(ProjectTask task);
    Task<IEnumerable<ProjectTask>> GetTasksByProjectId(Guid projectId);
    Task<IEnumerable<ProjectTask>> GetTasksByUserId(Guid userId);
    Task<IEnumerable<ProjectTask>> GetTasksByStageId(Guid processId);
    Task<ProjectTask?> GetById(Guid id);
    Task UpdateProperty<TProperty>(ProjectTask task,
        Expression<Func<ProjectTask, TProperty>> expression);
}