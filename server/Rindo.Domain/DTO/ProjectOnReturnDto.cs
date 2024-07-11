using Rindo.Domain.Entities;
using Task = Rindo.Domain.Entities.Task;

namespace Rindo.Domain.DTO;

public class ProjectOnReturnDto
{
    public Guid Id { get; set;}
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string InviteLink { get; init; } = default!;
    public UserDto Owner { get; set; } 
    public Chat Chat { get; set; } 
    public IEnumerable<UserDto> Users { get; set; } 
    public IEnumerable<RoleDto> Roles { get; set; } 
    public DateOnly StartDate { get; set; } 
    public DateOnly FinishDate { get; set; } 
}