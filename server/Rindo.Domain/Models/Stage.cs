namespace Rindo.Domain.Models;

public class Stage
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid ProjectId { get; set; }
    public IEnumerable<ProjectTask> Tasks { get; set; } = default!;
    public int Index { get; set; }
}