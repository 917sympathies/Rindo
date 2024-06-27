using System.Collections;

namespace Rindo.Domain.Entities;

public class Task
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Progress { get; set; }
    public int Index { get; set; }
    public Guid ProjectId { get; set; }
    public Guid StageId { get; set; }
    public Nullable<Guid> ResponsibleUserId { get; set; }
    public IEnumerable<TaskComment>? Comments { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? FinishDate { get; set; }
}