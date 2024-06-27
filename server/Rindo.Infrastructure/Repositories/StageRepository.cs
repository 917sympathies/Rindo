using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class StageRepository : RepositoryBase<Stage>, IStageRepository
{
    public StageRepository(RindoDbContext context) : base(context)
    {
    }

    public Task CreateStages(IEnumerable<Stage> stages) => CreateAsync(stages);

    public Task CreateStage(Stage stages) => CreateAsync(stages);

    public Task DeleteStage(Stage stages) => DeleteAsync(stages);

    public Task UpdateStage(Stage stages) => UpdateAsync(stages);

    public Task UpdateStageProperty<TProperty>(Stage stage, Expression<Func<Stage, TProperty>> expression) =>
        UpdateStageProperty<TProperty>(stage, expression);

    public async Task<Stage?> GetById(Guid id) => 
        await FindByCondition(p => p.Id == id)
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