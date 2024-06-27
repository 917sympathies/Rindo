namespace Rindo.Domain.DTO;

public class RolesRights
{
    public RolesRights()
    {
        
    }
    public RolesRights(bool _)
    {
        if (_)
        {
            CanAddTask = true;
            CanModifyTask = true;
            CanCompleteTask = true;
            CanDeleteTask = true;
            CanAddStage = true;
            CanModifyStage = true;
            CanDeleteStage = true;
            CanAddRoles = true;
            CanModifyRoles = true;
            CanInviteUser = true;
            CanExcludeUser = true;
            CanUseChat = true;
        };
    }
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