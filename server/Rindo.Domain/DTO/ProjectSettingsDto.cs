using Rindo.Domain.Models;

namespace Rindo.Domain.DTO;

public class ProjectSettingsDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string InviteLink { get; init; } = default!;
    public IEnumerable<User> Users { get; set; } = default!; //TODO: replace with DTOs
    public IEnumerable<Role> Roles { get; set; } = default!; //TODO: replace with DTOs
    public DateOnly StartDate { get; set; } 
    public DateOnly FinishDate { get; set; }
}