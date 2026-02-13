using Rindo.Domain.DTO.Roles;

namespace Rindo.Domain.DTO.Projects;

public class ProjectOnReturnDto
{
    public Guid Id { get; init;}
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public Guid OwnerId { get; init; }
    public UserDto[] Users { get; init; } = default!;
    public RoleDto[] Roles { get; init; } = default!;
    public DateTimeOffset Created { get; init; } 
    public DateTimeOffset? DeadlineDate { get; init; }
}