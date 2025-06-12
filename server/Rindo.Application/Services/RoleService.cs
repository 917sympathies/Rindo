using Application.Common.Mapping;
using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Enums;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure;

namespace Application.Services;

public class RoleService : IRoleService
{
    
    private readonly IRoleRepository _roleRepository;
    
    private readonly IUserRepository _userRepository;
    
    private readonly IProjectRepository _projectRepository;
    
    private readonly PostgresDbContext _context; //TODO: remove DbContext
    
    public RoleService(IRoleRepository roleRepository, IUserRepository userRepository, IProjectRepository projectRepository, PostgresDbContext context)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _context = context;
    }
    
    public async Task<Result> CreateRole(RoleDtoOnCreate roleDto)
    {
        var role = roleDto.MapToModel();
        await _roleRepository.CreateRole(role);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteRole(Guid id)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if(role is null) return Result.Failure(Error.NotFound("Role with this id doesn't exists"));
        _roleRepository.DeleteRole(role);
        return Result.Success();
    }

    public async Task<Result> UpdateRoleName(Guid id, string name)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if(role is null) return Result.Failure(Error.NotFound("User with this id doesn't exists"));
        role.Name = name;
        await _roleRepository.UpdateProperty(role, r => r.Name);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AddUserToRole(Guid id, Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if(user is null) return Result.Failure(Error.NotFound("User with this id doesn't exists"));
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if(role is null) return Result.Failure(Error.NotFound("Role with this id doesn't exists"));
        role.Users.Add(user);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> RemoveUserFromRole(Guid id, Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if(user is null) return Result.Failure(Error.NotFound("User with this id doesn't exists"));
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if(role is null) return Result.Failure(Error.NotFound("Role with this id doesn't exists"));
        
        await _context.Entry(role).Collection(p => p.Users).LoadAsync();
        role.Users.Remove(user);

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateRoleRights(Guid id, RoleRights rights)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if(role is null) return Result.Failure(Error.NotFound("Role with this id doesn't exists"));
        role.BitRoleRights = rights;
        _roleRepository.UpdateRole(role);
        return Result.Success();
    }

    public async Task<RoleRights> GetRightsByProjectId(Guid projectId, Guid userId)
    {
        var userTask = _userRepository.GetUserById(userId);
        var projectTask = _projectRepository.GetProjectById(projectId);
        await Task.WhenAll(userTask, projectTask);
        var user = userTask.Result ?? throw new ArgumentException("User not found");
        var project = projectTask.Result ?? throw new ArgumentException("Project not found");
        if (project.OwnerId == user.Id)
        {
            return (RoleRights)Enum.GetValues<RoleRights>().Cast<int>().Sum();
        }
        var roles =  _context.Roles.Where(r => r.Users.Contains(new User { Id = userId })).ToList();
        if (roles.Count == 0) return 0; 
        var rights = 0;
        foreach (var role in roles)
        {
            rights |= (int)role.BitRoleRights;
        }

        return (RoleRights)rights;
    }
    
    public async Task<IEnumerable<RoleDto>> GetRolesByProjectId(Guid projectId)
    {
        var roles = (await _roleRepository.GetRolesByProjectId(projectId)).ToList();
        return roles.Select(x => x.MapToDto());
    }

    private async Task<IEnumerable<Role>> GetRolesForUser(Guid projectId)
    {
        return await _roleRepository.GetRolesByProjectId(projectId);
    }
}