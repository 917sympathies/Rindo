using System.ComponentModel.DataAnnotations.Schema;

namespace Rindo.Domain.DataObjects;

[Table("invitations", Schema = "dbo")]
public class Invitation
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
}

public class InvitationDto
{
    public Guid InvitationId { get; set; }
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    public Guid SenderId { get; set; }
    public string SenderUsername { get; set; }
}