using Rindo.Domain.Entities;
using Task = Rindo.Domain.Entities.Task;

namespace Rindo.Domain.DTO;

public class ProjectOnReturnDto
{
    public Guid Id { get; init;}
    public string Name { get; init; } = default!;
    
    public string Description { get; init; } = default!;
    
    public string InviteLink { get; init; } = default!;
    
    public Guid OwnerId { get; init; }
    
    public IEnumerable<UserDto> Users { get; init; } = default!;
    
    public IEnumerable<RoleDto> Roles { get; init; } = default!;
    
    public DateOnly StartDate { get; init; } 
    
    public DateOnly FinishDate { get; init; } 
}