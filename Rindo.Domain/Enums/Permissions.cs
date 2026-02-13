namespace Rindo.Domain.Enums;


// TODO: redo it somehow because now permission list is limited
[Flags]
public enum Permissions
{
    Task_Add = 1 << 1,
    Task_Edit = 1 << 2,
    CanCompleteTask = 1 << 3,
    Task_Delete = 1 << 4,
    Stage_Add = 1 << 5,
    Stage_Edit = 1 << 6,
    Stage_Delete = 1 << 7,
    Roles_Add = 1 << 8,
    Roles_Edit = 1 << 9,
    Invite_Add = 1 << 10,
    CanExcludeUser = 1 << 11,
    CanUseChat = 1 << 12,
}