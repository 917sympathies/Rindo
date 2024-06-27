namespace Rindo.Domain.DTO;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public IEnumerable<UserDto> Users { get; set; } = default!;
    public bool CanAddTask { get; set; }
    public bool CanModifyTask { get; set; }
    public bool CanCompleteTask { get; set; }
    public bool CanDeleteTask { get; set; }
    public bool CanAddStage { get; set; }
    public bool CanModifyStage { get; set; }
    public bool CanDeleteStage { get; set; }
    public bool CanAddRoles { get; set; }
    public bool CanModifyRoles { get; set; }
    public bool CanInviteUser { get; set; }
    public bool CanExcludeUser { get; set; }
    public bool CanUseChat { get; set; }
}