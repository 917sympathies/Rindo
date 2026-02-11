namespace Rindo.Domain.DTO.Projects;

public class InvitationProjectInfoDto
{
    public Guid Id { get; set; }
    public string SenderUsername { get; set; }
    public string RecipientUsername { get; set; }
}