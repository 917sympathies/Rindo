using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.DataObjects;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class StageRepository(PostgresDbContext context) : RepositoryBase<Stage>(context), IStageRepository
{
    public Task CreateStages(IEnumerable<Stage> stages) => CreateAsync(stages);

    public Task<Stage> CreateStage(Stage stages) => CreateAsync(stages);

    public async Task DeleteStage(Stage stages) => await Delete(stages);

    public async Task UpdateStage(Stage stages) => await Update(stages);

    public async Task<Stage?> GetById(Guid id) => 
        await FindByCondition(p => p.StageId == id)
            .Include(s => s.Tasks)
            .ThenInclude(t => t.Comments)
            .AsNoTracking()
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Stage>> GetStagesByProjectId(Guid projectId) =>
        await FindByCondition(p => p.ProjectId == projectId)
            .Include(s => s.Tasks)
            // .ThenInclude(t => t.Comments)
            .OrderBy(s => s.Index)
            .ToListAsync();
}