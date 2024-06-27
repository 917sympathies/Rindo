namespace Rindo.Domain.DTO;

public class StageOnCreateDto
{
    public string Name { get; set; } = default!;
    public Guid ProjectId { get; set; }
}