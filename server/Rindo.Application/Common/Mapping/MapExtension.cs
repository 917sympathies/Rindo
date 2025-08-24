using Rindo.Domain.DTO;
using Rindo.Domain.Models;

namespace Application.Common.Mapping;

public static class MapExtension
{
    public static UserDto MapToDto(this User user)
    {
        return new UserDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Id = user.Id
        };
    }

    public static User MapToModel(this SignUpDto dto)
    {
        return new User
        {
            Username = dto.Username,
            Password = dto.Password,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };
    }

    public static RoleDto MapToDto(this Role role)
    {
        return new RoleDto
        {
            BitRolesRights = role.BitRoleRights,
            Name = role.Name,
            Id = role.Id,
            //Users = role.Users,
        };
    }

    public static ProjectOnReturnDto MapToDto(this Project project)
    {
        return new ProjectOnReturnDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            OwnerId = project.OwnerId,
            StartDate = project.CreatedDate,
            
            // do we actually need these properties in this dto?
            Roles = project.Roles.Select(x => x.MapToDto()).ToArray(),
            Users = project.Users.Select(x => x.MapToDto()).ToArray()
        };
    }

    public static ProjectHeaderInfoDto MapToHeaderDto(this Project project)
    {
        return new ProjectHeaderInfoDto
        {
            ChatId = project.ChatId,
            Name = project.Name,
            OwnerId = project.OwnerId
        };
    }

    public static ProjectShortInfoDto MapToSidebarDto(this Project project)
    {
        return new ProjectShortInfoDto
        {
            Id = project.Id,
            Name = project.Name
        };
    }

    public static Role MapToModel(this RoleDtoOnCreate dto)
    {
        return new Role
        {
            Name = dto.Name,
            ProjectId = dto.ProjectId,
            Color = dto.Color
        };
    }

    public static Stage MapToDto(this StageOnCreateDto dto)
    {
        return new Stage
        {
            ProjectId = dto.ProjectId,
            Name = dto.Name
        };
    }
}