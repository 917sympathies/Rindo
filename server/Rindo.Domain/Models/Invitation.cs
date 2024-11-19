namespace Rindo.Domain.Models;

public class Invitation
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    public Guid UserId { get; set; }
    public string SenderUsername { get; set; }
}