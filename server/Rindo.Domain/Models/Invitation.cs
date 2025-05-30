namespace Rindo.Domain.Models;

public class Invitation
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
}