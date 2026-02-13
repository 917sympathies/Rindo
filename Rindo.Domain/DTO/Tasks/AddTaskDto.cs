using Rindo.Domain.Enums;

namespace Rindo.Domain.DTO.Tasks;

public class AddTaskDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TaskPriority Priority { get; set; }
    public Guid ProjectId { get; set; }
    public Guid StageId { get; set; }
    public Guid? AssigneeId { get; set; }
    public Guid ReporterId { get; set; } 
    public DateTimeOffset? DeadlineDate { get; set; }
}