namespace Rindo.Domain.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
}
