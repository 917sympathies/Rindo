using Rindo.Domain.Common;
using Rindo.Domain.Models;

namespace Application.Interfaces.Services;

public interface ITaskService
{
    Task<Result> CreateTask(ProjectTask projectTask);
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
    Task<Result<object>>  GetTaskById(Guid id);
}