namespace Rindo.Domain.Models;

public class Project
{
    public Guid Id { get; init;}
    public string Name { get; set; } 
    public string? Description { get; set; }
    public Guid OwnerId { get; init; }
    public Guid ChatId { get; set; } 
    public ICollection<Stage> Stages { get; set; } 
    public ICollection<User> Users { get; set; } 
    public ICollection<Role> Roles { get; set; } 
    public ICollection<Invitation> Invitations { get; set; } 
    public ICollection<Tag> Tags { get; set; } 
    public DateOnly CreatedDate { get; set; } 
}