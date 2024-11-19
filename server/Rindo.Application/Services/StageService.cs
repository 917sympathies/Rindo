using Application.Interfaces.Services;
using Application.Mapping;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;

namespace Application.Services;

public class StageService : IStageService
{
    private readonly IStageRepository _stageRepository;
    
    private readonly ITaskRepository _taskRepository;
    
    private readonly RindoDbContext _context;
    
    public StageService(IStageRepository stageRepository, ITaskRepository taskRepository, RindoDbContext context)
    {
        _stageRepository = stageRepository;
        _taskRepository = taskRepository;
        _context = context;
    }

    public async Task<Result> AddStage(StageOnCreateDto stageDto)
    {
        var stage = stageDto.MapToDto();
        stage.Index = (await _stageRepository.GetStagesByProjectId(stageDto.ProjectId)).Max(s => s.Index) + 1;
        try
        {
            await _stageRepository.CreateStage(stage);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.Failure(e.Message));
        }
        return Result.Success();
    }

    public async Task<Result<string>> GetStageName(Guid stageId)
    {
        var stage = await _stageRepository.GetById(stageId);
        if (stage is null) return Error.NotFound("Stage with this id doesn't exists");
        return stage.Name;
    }

    public async Task<IEnumerable<Stage>> GetStagesByProjectId(Guid projectId)
    {
        var stages = await _stageRepository.GetStagesByProjectId(projectId);
        return stages;
    }

    public async Task<Result> ChangeStageTask(Guid id, Guid taskId)
    {
        var task = await _taskRepository.GetById(taskId);
        if (task is null) return Error.NotFound("Task with this id doesn't exists");
        var stage = await _stageRepository.GetById(id);
        if(stage is null) return Error.NotFound("Stage with this id doesn't exists");
        task.StageId = stage.Id;
        var t = task.GetType().GetProperty(nameof(task.StageId));
        await _taskRepository.UpdateProperty(task, t => t.StageId);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteStage(Guid id)
    {
        var stage = await _stageRepository.GetById(id);
        if (stage is null) return Result.Failure(Error.NotFound("Stage with this id doesn't exists"));
        try
        {
            
            await _stageRepository.DeleteStage(stage);
            await _context.SaveChangesAsync();
            var stages = _context.Stages.Where(st => st.Index >= stage.Index).ToList();
            if (stages.Count > 0)
            {
                var index = stages.Max(s => s.Index);
                if (index != stage.Index)
                    foreach (var st in stages)
                        st.Index -= 1;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            return Result.Failure(Error.Failure(e.Message));
        }
        return Result.Success();
    }
}