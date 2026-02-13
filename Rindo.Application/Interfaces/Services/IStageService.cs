using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Roles;
using Rindo.Domain.DTO.Tasks;
using Rindo.Domain.DataObjects;

namespace Application.Interfaces.Services;

public interface IStageService
{
    Task<Stage> AddStage(StageOnCreateDto stageDto);
    Task DeleteStage(Guid stageId);
    Task<IEnumerable<StageDto>> GetStagesByProjectId(Guid projectId);
}