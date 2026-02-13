using Rindo.Domain.DTO.Tasks;
using Rindo.Domain.Enums;

namespace Rindo.Domain.DTO.Projects;

public class ProjectTaskDto
{
    public Guid TaskId { get; set; }
    public required string Name { get; set; }
    public string TaskNumber { get; set; }
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public int Index { get; set; }
    public Guid ProjectId { get; set; }
    public Guid StageId { get; set; }
    public UserShortInfoDto? Assignee { get; set; }
    public UserShortInfoDto Reporter { get; set; } // Creator
    public DateTimeOffset Created { get; set; }
    public TaskCommentDto[]? Comments { get; set; }
    public DateTimeOffset? DeadlineDate { get; set; }
}

public class UpdateTaskDto
{
    public Guid TaskId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public int Index { get; set; }
    public Guid StageId { get; set; }
    public Guid? AssigneeId { get; set; }
    public DateTimeOffset? DeadlineDate { get; set; }
}