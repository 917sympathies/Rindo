using System.Linq.Expressions;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<ProjectTask> CreateTask(ProjectTask projectTask);
    void DeleteTask(ProjectTask projectTask);
    void UpdateTask(ProjectTask projectTask);
    Task UnassignTasksFromUser(Guid projectId, Guid userId);
    Task<IEnumerable<ProjectTask>> GetTasksByProjectId(Guid projectId);
    Task<IEnumerable<ProjectTask>> GetTasksInProjectAssignedToUser(Guid projectId, Guid userId);
    Task<IEnumerable<ProjectTask>> GetTasksAssignedToUser(Guid userId);
    Task<IEnumerable<ProjectTask>> GetTasksByStageId(Guid processId);
    Task<ProjectTask?> GetById(Guid id);
    Task UpdateProperty<TProperty>(ProjectTask projectTask, Expression<Func<ProjectTask, TProperty>> expression);
}