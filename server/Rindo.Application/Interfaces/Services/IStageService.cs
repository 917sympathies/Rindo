using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;

namespace Application.Interfaces.Services;

public interface IStageService
{
    Task<Stage> AddStage(StageOnCreateDto stageDto);
    Task DeleteStage(Guid stageId, Guid projectId);
    Task<string> GetStageName(Guid stageId);
    Task<IEnumerable<Stage>> GetStagesByProjectId(Guid projectId);
    Task ChangeStageTask(Guid stageId, Guid taskId);
}