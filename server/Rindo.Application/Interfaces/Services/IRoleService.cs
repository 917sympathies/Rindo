using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Enums;

namespace Application.Interfaces.Services;

public interface IRoleService
{
    Task CreateRole(RoleDtoOnCreate role);
    Task DeleteRole(Guid roleId);
    Task UpdateRoleName(Guid roleId, string name);
    Task AddUserToRole(Guid roleId, Guid userId);
    Task RemoveUserFromRole(Guid roleId, Guid userId);
    Task UpdateRoleRights(Guid roleId, RoleRights rights);
    Task<RoleRights> GetRightsByProjectId(Guid projectId, Guid userID);
    Task<IEnumerable<RoleDto>> GetRolesByProjectId(Guid projectId);
}