namespace Rindo.Domain.DTO.Projects;

public class ProjectOnCreateDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? DeadlineDate { get; set; }
}