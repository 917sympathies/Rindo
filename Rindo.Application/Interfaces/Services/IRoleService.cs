using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Roles;
using Rindo.Domain.Enums;

namespace Application.Interfaces.Services;

public interface IRoleService
{
    Task CreateRole(RoleDtoOnCreate role);
    Task DeleteRole(Guid roleId);
    Task UpdateRoleName(Guid roleId, string name);
    Task AddUserToRole(Guid roleId, Guid userId);
    Task RemoveUserFromRole(Guid roleId, Guid userId);
    Task RemoveRolesByProjectId(Guid projectId);
    Task UpdateRoleRights(Guid roleId, Permissions rights);
    Task<Permissions> GetRightsByProjectId(Guid projectId, Guid userId);
    Task<IEnumerable<RoleDto>> GetRolesByProjectId(Guid projectId);
}