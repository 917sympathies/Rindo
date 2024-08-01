namespace Rindo.Domain.Entities;

public class Tag
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ProjectId { get; init; }
}