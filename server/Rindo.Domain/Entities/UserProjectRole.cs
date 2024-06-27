namespace Rindo.Domain.Entities;

public class UserProjectRole
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;
}