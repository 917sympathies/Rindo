using Rindo.Domain.Enums;

namespace Rindo.Domain.DTO.Roles;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public IEnumerable<UserDto> Users { get; set; } = default!;
    public Permissions BitRolesRights { get; set; }
}