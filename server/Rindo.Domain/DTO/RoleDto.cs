using Rindo.Domain.Enums;

namespace Rindo.Domain.DTO;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public IEnumerable<UserDto> Users { get; set; } = default!;
    public RoleRights BitRolesRights { get; set; }
}