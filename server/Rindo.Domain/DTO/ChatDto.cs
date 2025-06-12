namespace Rindo.Domain.DTO;

public class ChatDto
{
    public Guid Id { get; set; }
    
    public MessageDto[] Messages { get; set; }
}