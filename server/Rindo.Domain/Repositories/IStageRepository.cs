using System.Linq.Expressions;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface IStageRepository
{
    Task CreateStages(IEnumerable<Stage> stages);
    Task CreateStage(Stage stages);
    Task DeleteStage(Stage stages);
    Task UpdateStage(Stage stages);
    Task<Stage?> GetById(Guid id);
    Task<IEnumerable<Stage>> GetStagesByProjectId(Guid projectId);
}