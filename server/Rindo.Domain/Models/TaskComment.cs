namespace Rindo.Domain.Models;

public class TaskComment
{
    public Guid Id { get; set; }
    public ProjectTask ProjectTask { get; set; } = default!;
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = default!;
    public DateTime Time { get; set; }
}