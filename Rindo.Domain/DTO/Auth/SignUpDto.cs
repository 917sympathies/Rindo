using System.ComponentModel.DataAnnotations;

namespace Rindo.Domain.DTO.Auth;

public class SignUpDto
{
    [Required] 
    [MaxLength(12)]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}