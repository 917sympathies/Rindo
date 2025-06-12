namespace Rindo.Domain.DTO;

public class ProjectOnReturnDto
{
    public Guid Id { get; init;}
    
    public string Name { get; init; } = default!;
    
    public string Description { get; init; } = default!;
    
    public Guid OwnerId { get; init; }
    
    public UserDto[] Users { get; init; } = default!;
    
    public RoleDto[] Roles { get; init; } = default!;
    
    public DateOnly StartDate { get; init; } 
    
    public DateOnly FinishDate { get; init; } 
}