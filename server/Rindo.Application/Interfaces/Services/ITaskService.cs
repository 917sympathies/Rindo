using Rindo.Domain.Common;

namespace Application.Interfaces.Services;

using ProjectTask = Rindo.Domain.Entities.Task;

public interface ITaskService
{
    Task<Result> CreateTask(ProjectTask task);
    Task<Result> UpdateName(Guid id, string name);
    Task<Result> UpdateDescription(Guid id,string description);
    Task<Result> UpdateResponsible(Guid id, Nullable<Guid> userId);
    Task<Result> UpdateStartDate(Guid id, DateOnly date);
    Task<Result> UpdateFinishDate(Guid id, DateOnly date);
    Task<Result> UpdateProgress(Guid id, string number);
    Task<Result> DeleteTask(Guid id);
    Task<IEnumerable<ProjectTask>> GetTasksByStageId(Guid stageId);
    Task<IEnumerable<object>> GetTasksByProjectId(Guid projectId);
    Task<IEnumerable<ProjectTask>> GetTasksByUserId(Guid userId);
    Task<Result<ProjectTask?>>  GetTaskById(Guid id);
}