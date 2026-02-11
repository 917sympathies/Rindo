namespace Rindo.Domain.DTO.Projects;

public class ChatDto
{
    public Guid Id { get; set; }
    public MessageDto[] Messages { get; set; }
}