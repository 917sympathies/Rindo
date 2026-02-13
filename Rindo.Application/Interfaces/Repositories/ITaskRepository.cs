using System.Linq.Expressions;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;
using Rindo.Domain.DataObjects;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<ProjectTask> CreateTask(ProjectTask projectTask);
    Task DeleteTask(ProjectTask projectTask);
    Task<int> GetNextTaskNumber(Guid projectId);
    Task UpdateTask(UpdateTaskDto projectTask, Guid modifiedBy, DateTime modified);
    Task UnassignTasksFromUser(Guid projectId, Guid userId);
    Task UpdateTaskStage(Guid taskId, Guid stageId);
    Task DeleteTasksByProjectId(Guid projectId);
    Task<IEnumerable<ProjectTask>> GetTasksByProjectId(Guid projectId);
    Task<IEnumerable<ProjectTask>> GetTasksInProjectAssignedToUser(Guid projectId, Guid userId);
    Task<IEnumerable<ProjectTask>> GetTasksAssignedToUser(Guid userId);
    Task<IEnumerable<ProjectTask>> GetTasksByStageId(Guid processId);
    Task<ProjectTask?> GetById(Guid id);
    Task UpdateProperty<TProperty>(ProjectTask projectTask, Expression<Func<ProjectTask, TProperty>> expression);
}