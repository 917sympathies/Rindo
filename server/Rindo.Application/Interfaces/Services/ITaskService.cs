using Rindo.Domain.Common;
using Rindo.Domain.Models;

namespace Application.Interfaces.Services;

public interface ITaskService
{
    Task<ProjectTask> CreateTask(ProjectTask projectTask);
    Task UpdateName(Guid taskId, string name);
    Task UpdateDescription(Guid taskId,string description);
    Task UpdateResponsible(Guid taskId, Nullable<Guid> userId);
    Task UpdateStartDate(Guid taskId, DateOnly date);
    Task UpdateFinishDate(Guid taskId, DateOnly date);
    Task UpdateProgress(Guid taskId, string number);
    Task DeleteTask(Guid taskId);
    Task<IEnumerable<ProjectTask>> GetTasksByStageId(Guid stageId);
    Task<IEnumerable<object>> GetTasksByProjectId(Guid projectId);
    Task<IEnumerable<ProjectTask>> GetTasksByUserId(Guid userId);
    Task<object>  GetTaskById(Guid id);
}