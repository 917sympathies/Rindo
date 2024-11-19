namespace Rindo.Domain.Models;

public class Tag
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ProjectId { get; init; }
}