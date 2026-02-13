using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rindo.Domain.Enums;

namespace Rindo.Domain.DataObjects;

[Table("project_tasks", Schema = "dbo")]
public class ProjectTask
{
    [Key]
    public Guid TaskId { get; set; }
    public string TaskNumber { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public int Index { get; set; }
    public Guid ProjectId { get; set; }
    public Guid StageId { get; set; }
    public Guid? AssigneeId { get; set; }
    public Guid ReporterId { get; set; } // Creator
    public DateTimeOffset Created { get; set; }
    public IEnumerable<TaskComment>? Comments { get; set; }
    public Guid ModifiedBy { get; set; }
    public DateTimeOffset Modified { get; set; } 
    public DateTimeOffset? DeadlineDate { get; set; }
}