using Application.Common.Exceptions;
using Application.Common.Mapping;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;

namespace Application.Services;

public class StageService(IStageRepository stageRepository, ITaskRepository taskRepository) : IStageService
{
    public async Task<Stage> AddStage(StageOnCreateDto stageDto)
    {
        var stage = stageDto.MapToDto();
        stage.Index = (await stageRepository.GetStagesByProjectId(stageDto.ProjectId)).Max(s => s.Index) + 1;
        return await stageRepository.CreateStage(stage);
    }

    public async Task<string> GetStageName(Guid stageId)
    {
        var stage = await stageRepository.GetById(stageId);
        if (stage is null) throw new NotFoundException(nameof(Stage), stageId);
        return stage.Name;
    }

    public async Task<IEnumerable<Stage>> GetStagesByProjectId(Guid projectId)
    {
        var stages = await stageRepository.GetStagesByProjectId(projectId);
        return stages;
    }

    public async Task ChangeStageTask(Guid stageId, Guid taskId)
    {
        var task = await taskRepository.GetById(taskId);
        if (task is null) throw new NotFoundException(nameof(ProjectTask), taskId);
        var stage = await stageRepository.GetById(stageId);
        if(stage is null) throw new NotFoundException(nameof(Stage), stageId);
        task.StageId = stage.Id;
        await taskRepository.UpdateProperty(task, pt => pt.StageId);
    }

    public async Task DeleteStage(Guid stageId, Guid projectId)
    {
        var stage = await stageRepository.GetById(stageId);
        if (stage is null) throw new NotFoundException(nameof(Stage), stageId);
        stageRepository.DeleteStage(stage);
        // TODO: rework this
        // var stages = _context.Stages.Where(st => st.Index >= stage.Index).ToList();
        // if (stages.Count > 0)
        // {
        //     var index = stages.Max(s => s.Index);
        //     if (index != stage.Index)
        //         foreach (var st in stages)
        //             st.Index -= 1;
        // }
    }
}