using Rindo.Domain.DTO.Projects;

namespace Rindo.Domain.DTO.Users;

public class UserProjectsTasks
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    public ProjectTaskDto[] Tasks { get; set; }
}