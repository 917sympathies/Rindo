using System.ComponentModel.DataAnnotations;

namespace Rindo.Domain.DTO;

public class SignUpDto
{
    [Required (ErrorMessage = "Необходимо указать ваше имя пользователя!")] 
    [MaxLength(12)]
    public string Username { get; set; } = default!;
    
    [Required (ErrorMessage = "Не забывайте про пароль!")]
    public string Password { get; set; } = default!;
    
    [EmailAddress (ErrorMessage = "Вы ввели некорректный Email")]
    public string Email { get; set; } = default!;
    
    public string FirstName { get; set; } = default!;
    
    public string LastName { get; set; } = default!;
}