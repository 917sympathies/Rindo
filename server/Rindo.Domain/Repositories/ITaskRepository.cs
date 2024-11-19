using System.Linq.Expressions;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface ITaskRepository
{
    Task CreateTask(ProjectTask projectTask);
    Task DeleteTask(ProjectTask projectTask);
    Task UpdateTask(ProjectTask projectTask);
    Task<IEnumerable<ProjectTask>> GetTasksByProjectId(Guid projectId);
    Task<IEnumerable<ProjectTask>> GetTasksByUserId(Guid userId);
    Task<IEnumerable<ProjectTask>> GetTasksByStageId(Guid processId);
    Task<ProjectTask?> GetById(Guid id);
    Task UpdateProperty<TProperty>(ProjectTask projectTask,
        Expression<Func<ProjectTask, TProperty>> expression);
}