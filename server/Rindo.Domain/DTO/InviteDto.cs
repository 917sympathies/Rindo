namespace Rindo.Domain.DTO;

public class InviteDto
{
    public Guid Id { get; set; }
    public string SenderUsername { get; set; }
    public string RecipientUsername { get; set; }
}