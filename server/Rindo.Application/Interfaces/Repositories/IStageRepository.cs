using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface IStageRepository
{
    Task CreateStages(IEnumerable<Stage> stages);
    Task<Stage> CreateStage(Stage stages);
    void DeleteStage(Stage stages);
    void UpdateStage(Stage stages);
    Task<Stage?> GetById(Guid id);
    Task<IEnumerable<Stage>> GetStagesByProjectId(Guid projectId);
}