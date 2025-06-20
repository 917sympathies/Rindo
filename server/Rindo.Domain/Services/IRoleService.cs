﻿using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Enums;

namespace Application.Interfaces.Services;

public interface IRoleService
{
    Task<Result> CreateRole(RoleDtoOnCreate role);
    Task<Result> DeleteRole(Guid id);
    Task<Result> UpdateRoleName(Guid id, string name);
    Task<Result> AddUserToRole(Guid id, Guid userId);
    Task<Result> RemoveUserFromRole(Guid id, Guid userId);
    Task<Result> UpdateRoleRights(Guid id, RoleRights rights);
    Task<RoleRights> GetRightsByProjectId(Guid projectId, Guid userID);
    Task<IEnumerable<RoleDto>> GetRolesByProjectId(Guid projectId);
}