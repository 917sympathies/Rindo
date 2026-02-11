namespace Rindo.Domain.DTO.Projects;

public class UpdateProjectDto
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}