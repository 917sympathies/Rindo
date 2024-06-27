using AutoMapper;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;

namespace Application.Services.RoleService;

public class RoleService : IRoleService
{
    private readonly IUserProjectRoleRepository _userProjectRoleRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository, IProjectRepository projectRepository, IUserProjectRoleRepository userProjectRoleRepository)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _userProjectRoleRepository = userProjectRoleRepository;
    }
    
    public async Task<Result> CreateRole(RoleDtoOnCreate roleDto)
    {
        var role = _mapper.Map<Role>(roleDto);
        await _roleRepository.CreateRole(role);
        await _unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteRole(Guid id)
    {
        var role = await _roleRepository.GetRoleById(id);
        if (role is null) return Error.NotFound("Нет такой роли!");
        await _roleRepository.DeleteRole(role);
        await _unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateRoleName(Guid id, string name)
    {
        var role = await _roleRepository.GetRoleById(id);
        if (role is null) return Error.NotFound("Нет такой роли!");
        role.Name = name;
        await _roleRepository.UpdateProperty(role, r => r.Name);
        await _unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<Result> AddUserToRole(Guid id, Guid userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if(user is null) return Result.Failure(Error.NotFound("Нет такого пользователя!"));
        var role = await _roleRepository.GetRoleById(id);
        if(role is null) return Result.Failure(Error.NotFound("Нет такой роли!"));
        var relation = new UserProjectRole() { ProjectId = role.ProjectId, RoleId = role.Id, UserId = user.Id };
        await _userProjectRoleRepository.CreateRelation(relation);
        role.UserProjectRoles.Add(relation);
        await _unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<Result> RemoveUserFromRole(Guid id, Guid userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if(user is null) return Result.Failure(Error.NotFound("Нет такого пользователя!"));
        var role = await _roleRepository.GetRoleById(id);
        if(role is null) return Result.Failure(Error.NotFound("Нет такой роли!"));
        var relation = await _userProjectRoleRepository.GetRelationByIds(role.ProjectId, role.Id, user.Id);
        await _userProjectRoleRepository.DeleteRelation(relation);
        await _unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateRoleRights(Guid id, RolesRights rights)
    {
        var role = await _roleRepository.GetRoleById(id);
        if (role is null) return Result.Failure(Error.NotFound("Такой роли нет!"));
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
        //await _roleRepository.UpdateProperty(role, r => r.CanAddTask);
        await _roleRepository.UpdateRole(role);
        await _unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<RolesRights> GetRightsByProjectId(Guid projectId, Guid userId)
    {
        // var projRoles = await GetRolesForUser(projectId);
        var user = await _userRepository.GetUserById(userId);
        var project = await _projectRepository.GetProjectById(projectId);
        if (user is null || project is null) return null;
        if (project.OwnerId == user.Id)
            return new RolesRights(true);
        var userProjectRoles = await _userProjectRoleRepository.GetRelationsByUserId(projectId, userId);
        var roles = userProjectRoles.Select(u => u.Role).ToList();
        // var roles = projRoles.Where(p => p.UserProjectRoles.Contains(new UserProjectRole() { UserId = user.Id, User = user})).ToList();
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
        foreach (var r in roles)
        {
            var users = await _userProjectRoleRepository.GetRelationsByProjectId(r.ProjectId, r.Id);
            r.Users = (users.Select(u => u.User));
        }

        var t = roles;
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    private async Task<IEnumerable<Role>> GetRolesForUser(Guid projectId)
    {
        return await _roleRepository.GetRolesByProjectId(projectId);
    }
}