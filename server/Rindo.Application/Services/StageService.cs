using Application.Common.Exceptions;
using Application.Common.Mapping;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Roles;
using Rindo.Domain.DTO.Tasks;
using Rindo.Domain.Enums;
using Rindo.Domain.DataObjects;

namespace Application.Services;

public class StageService(IStageRepository stageRepository, IUserRepository userRepository) : IStageService
{
    public async Task<Stage> AddStage(StageOnCreateDto stageDto)
    {
        var stage = stageDto.MapToDto();
        stage.Index = (await stageRepository.GetStagesByProjectId(stageDto.ProjectId)).Max(s => s.Index) + 1;
        return await stageRepository.CreateStage(stage);
    }

    public async Task<IEnumerable<StageDto>> GetStagesByProjectId(Guid projectId)
    {
        var stages = (await stageRepository.GetStagesByProjectId(projectId)).ToArray();
        var assigneeUsersIds = stages.Where(x => x.Tasks.Any()).SelectMany(x => x.Tasks)
            .Where(x => x.AssigneeId.HasValue).Select(x => x.AssigneeId!.Value).ToArray();
        var users = await userRepository.GetUsersByIds(assigneeUsersIds);
        
        return stages.Select(s => s.MapToDto()).Select(s =>
        {
            foreach(var t in s.Tasks.Where(x => x.Assignee is not null))
            {
                var assignee = users.First(x => x.UserId == t.Assignee!.Id);
                t.Assignee.FirstName = assignee.FirstName;
                t.Assignee.LastName = assignee.LastName;
            }
            return s;
        }).ToArray();
    }

    public async Task DeleteStage(Guid stageId)
    {
        var stage = await stageRepository.GetById(stageId);
        if (stage is null) throw new NotFoundException(nameof(Stage), stageId);
        if (stage.Type == StageType.Custom) throw new ArgumentException($"Stage with id=${stageId} is not custom");
        await stageRepository.DeleteStage(stage);
    }
}