namespace Rindo.Domain.DTO.Projects;

public class ProjectHeaderInfoDto
{
    public string Name { get; set; }
    public Guid OwnerId { get; set; }
    public Guid ChatId { get; set; }
}