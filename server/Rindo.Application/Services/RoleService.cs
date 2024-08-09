using Application.Interfaces.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;

namespace Application.Services;

public class RoleService : IRoleService
{
    
    private readonly IRoleRepository _roleRepository;
    
    private readonly IUserRepository _userRepository;
    
    private readonly IProjectRepository _projectRepository;
    
    private readonly IMapper _mapper;
    
    private readonly RindoDbContext _context;
    
    public RoleService(IRoleRepository roleRepository, IMapper mapper, IUserRepository userRepository, IProjectRepository projectRepository, RindoDbContext context)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _context = context;
    }
    
    public async Task<Result> CreateRole(RoleDtoOnCreate roleDto)
    {
        var role = _mapper.Map<Role>(roleDto);
        await _roleRepository.CreateRole(role);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteRole(Guid id)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if(role is null) return Result.Failure(Error.NotFound("Нет такой роли!"));
        await _roleRepository.DeleteRole(role);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateRoleName(Guid id, string name)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if(role is null) return Result.Failure(Error.NotFound("Нет такой роли!"));
        role.Name = name;
        await _roleRepository.UpdateProperty(role, r => r.Name);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AddUserToRole(Guid id, Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if(user is null) return Result.Failure(Error.NotFound("Нет такого пользователя!"));
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if(role is null) return Result.Failure(Error.NotFound("Нет такой роли!"));
        role.Users.Add(user);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> RemoveUserFromRole(Guid id, Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if(user is null) return Result.Failure(Error.NotFound("Нет такого пользователя!"));
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if(role is null) return Result.Failure(Error.NotFound("Нет такой роли!"));
        
        await _context.Entry(role).Collection(p => p.Users).LoadAsync();
        role.Users.Remove(user);

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateRoleRights(Guid id, RolesRights rights)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if(role is null) return Result.Failure(Error.NotFound("Нет такой роли!"));
        role.CanAddRoles = rights.CanAddRoles;
        role.CanAddStage = rights.CanAddStage;
        role.CanDeleteStage = rights.CanDeleteStage;
        role.CanAddTask = rights.CanAddTask;
        role.CanDeleteTask = rights.CanDeleteTask;
        role.CanModifyRoles = rights.CanModifyRoles;
        role.CanCompleteTask = rights.CanCompleteTask;
        role.CanExcludeUser = rights.CanExcludeUser;
        role.CanInviteUser = rights.CanInviteUser;
        role.CanModifyStage = rights.CanModifyStage;
        role.CanModifyTask = rights.CanModifyTask;
        role.CanUseChat = rights.CanUseChat;
        await _roleRepository.UpdateRole(role);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<RolesRights> GetRightsByProjectId(Guid projectId, Guid userId)
    {
        var user = await _userRepository.GetUserById(userId);
        var project = await _projectRepository.GetProjectById(projectId);
        if (user is null || project is null) return null;
        if (project.OwnerId == user.Id)
            return new RolesRights(true);
        var roles =  _context.Roles.Where(r => r.Users.Contains(new User { Id = userId })).ToList();
        if (roles.Count == 0) return new RolesRights(); 
        var rights = new RolesRights();
        foreach (var role in roles)
        {
            rights.CanAddTask |= role.CanAddTask;
            rights.CanModifyTask |= role.CanModifyTask;
            rights.CanCompleteTask |= role.CanCompleteTask;
            rights.CanDeleteTask |= role.CanDeleteTask;
            rights.CanAddStage |= role.CanAddStage;
            rights.CanModifyStage |= role.CanModifyStage;
            rights.CanDeleteStage |= role.CanDeleteStage;
            rights.CanAddRoles |= role.CanAddRoles;
            rights.CanModifyRoles |= role.CanModifyRoles;
            rights.CanInviteUser |= role.CanInviteUser;
            rights.CanExcludeUser |= role.CanExcludeUser;
            rights.CanUseChat |= role.CanUseChat;
        }

        return rights;
    }
    
    public async Task<IEnumerable<RoleDto>> GetRolesByProjectId(Guid projectId)
    {
        var roles = (await _roleRepository.GetRolesByProjectId(projectId)).ToList();
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    private async Task<IEnumerable<Role>> GetRolesForUser(Guid projectId)
    {
        return await _roleRepository.GetRolesByProjectId(projectId);
    }
}