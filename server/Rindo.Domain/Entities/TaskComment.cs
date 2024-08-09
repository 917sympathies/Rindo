namespace Rindo.Domain.Entities;

public class TaskComment
{
    public Guid Id { get; set; }
    public Task Task { get; set; } = default!;
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = default!;
    public DateTime Time { get; set; }
}