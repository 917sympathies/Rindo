using Rindo.Domain.Entities;

namespace Rindo.Domain.DTO;

public class ProjectSettingsDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string InviteLink { get; init; } = default!;
    public IEnumerable<User> Users { get; set; } = default!;
    public IEnumerable<Role> Roles { get; set; } = default!;
    public DateOnly StartDate { get; set; } 
    public DateOnly FinishDate { get; set; }
}