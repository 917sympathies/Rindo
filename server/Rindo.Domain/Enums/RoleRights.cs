namespace Rindo.Domain.Enums;

[Flags]
public enum RoleRights
{
    // make it flags enum
    CanAddTask = 1 << 1,
    CanModifyTask = 1 << 2,
    CanCompleteTask = 1 << 3,
    CanDeleteTask = 1 << 4,
    CanAddStage = 1 << 5,
    CanModifyStage = 1 << 6,
    CanDeleteStage = 1 << 7,
    CanAddRoles = 1 << 8,
    CanModifyRoles = 1 << 9,
    CanInviteUser = 1 << 10,
    CanExcludeUser = 1 << 11,
    CanUseChat = 1 << 12,
}