using Application.Common.Exceptions;
using Application.Common.Mapping;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Roles;
using Rindo.Domain.Enums;
using Rindo.Domain.DataObjects;

namespace Application.Services;

public class RoleService(
    IRoleRepository roleRepository,
    IUserService userService,
    IProjectService projectService)
    : IRoleService
{
    public async Task CreateRole(RoleDtoOnCreate roleDto)
    {
        var role = roleDto.MapToModel();
        await roleRepository.CreateRole(role);
    }

    public async Task DeleteRole(Guid roleId)
    {
        var role = await roleRepository.GetRoleById(roleId);
        if(role is null) throw new NotFoundException(nameof(Role), roleId);
        await roleRepository.DeleteRole(role);
    }

    public async Task UpdateRoleName(Guid roleId, string name)
    {
        var role = await roleRepository.GetRoleById(roleId);
        if(role is null) throw new NotFoundException(nameof(Role), roleId);
        role.Name = name;
        await roleRepository.UpdateProperty(role, r => r.Name);
    }

    public async Task AddUserToRole(Guid roleId, Guid userId)
    {
        var user = await userService.GetUserById(userId);
        if(user is null) throw new NotFoundException(nameof(User), userId);
        var role = await roleRepository.GetRoleById(roleId);
        if(role is null) throw new NotFoundException(nameof(Role), roleId);
        await roleRepository.AddUserToRole(roleId, userId);
    }

    public async Task RemoveUserFromRole(Guid roleId, Guid userId)
    {
        await roleRepository.RemoveUserFromRole(roleId, userId);
    }

    public async Task RemoveRolesByProjectId(Guid projectId)
    {
        await roleRepository.RemoveRolesByProjectId(projectId);
    }

    public async Task UpdateRoleRights(Guid roleId, Permissions rights)
    {
        var role = await roleRepository.GetRoleById(roleId);
        if(role is null) throw new NotFoundException(nameof(Role), roleId);
        role.BitPermissions = rights;
        await roleRepository.UpdateRole(role);
    }

    public async Task<Permissions> GetRightsByProjectId(Guid projectId, Guid userId)
    {
        var user = await userService.GetUserById(userId) ?? throw new NotFoundException(nameof(User), userId);
        var project = await projectService.GetProjectById(projectId) ?? throw new NotFoundException(nameof(Project), projectId);
        if (project.OwnerId == user.Id)
        {
            return (Permissions)Enum.GetValues<Permissions>().Cast<int>().Sum();
        }
        var roles = await roleRepository.GetRolesByUserId(userId);
        if (roles.Length == 0) return 0;
        
        return (Permissions)roles.Aggregate(0, (current, role) => current | (int)role.BitPermissions);
    }
    
    public async Task<IEnumerable<RoleDto>> GetRolesByProjectId(Guid projectId)
    {
        var roles = (await roleRepository.GetRolesByProjectId(projectId)).ToList();
        return roles.Select(x => x.MapToDto());
    }

    private async Task<IEnumerable<Role>> GetRolesForUser(Guid projectId)
    {
        return await roleRepository.GetRolesByProjectId(projectId);
    }
}