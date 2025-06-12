using Rindo.Domain.Enums;

namespace Rindo.Domain.Models;

public class ProjectTask
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Progress { get; set; }
    public TaskPriority Priority { get; set; }
    public int Index { get; set; }
    public Guid ProjectId { get; set; }
    public Guid StageId { get; set; }
    public Guid? AsigneeUserId { get; set; }
    public Guid ReporterUserId { get; set; }
    public TaskComment[] Comments { get; set; }
    public DateOnly? CreatedDate { get; set; }
    public DateOnly? FinishDate { get; set; } 
}