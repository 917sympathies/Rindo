namespace Rindo.Domain.Models;

public class ProjectTask
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Progress { get; set; }
    public int Index { get; set; }
    public Guid ProjectId { get; set; }
    public Guid StageId { get; set; }
    public Guid? ResponsibleUserId { get; set; }
    public IEnumerable<TaskComment>? Comments { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? FinishDate { get; set; } 
}