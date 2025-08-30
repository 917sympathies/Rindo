using System.ComponentModel.DataAnnotations.Schema;
using Rindo.Domain.Enums;
using TaskStatus = Rindo.Domain.Enums.TaskStatus;

namespace Rindo.Domain.Models;

[Table("ProjectTasks", Schema = "dbo")]
public class ProjectTask
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public int Index { get; set; }
    public Guid ProjectId { get; set; }
    public Guid StageId { get; set; }
    public Guid? AssigneeId { get; set; }
    public Guid ReporterId { get; set; }
    public IEnumerable<TaskComment> Comments { get; set; }
    public DateOnly? CreatedDate { get; set; }
    public DateOnly? UpdatedDate { get; set; } 
}