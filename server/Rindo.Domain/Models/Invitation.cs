using System.ComponentModel.DataAnnotations.Schema;

namespace Rindo.Domain.Models;

[Table("Invitations", Schema = "dbo")]
public class Invitation
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
}