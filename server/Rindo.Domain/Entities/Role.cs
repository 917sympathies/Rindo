using System.Collections;
using System.Runtime.Serialization;

namespace Rindo.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Color { get; set; } = default!;
    [IgnoreDataMember]
    public IEnumerable<User> Users { get; set; } = new List<User>();
    public ICollection<UserProjectRole> UserProjectRoles { get; set; } = default!;
    public Guid ProjectId { get; set; }
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