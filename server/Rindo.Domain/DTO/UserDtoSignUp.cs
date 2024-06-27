using System.ComponentModel.DataAnnotations;

namespace Rindo.Domain.DTO;

public class UserDtoSignUp
{
    [Required (ErrorMessage = "Необходимо указать ваше имя пользователя!")] 
    [MaxLength(12)]
    public string Username { get; set; } = default!;
    [Required (ErrorMessage = "Не забывайте про пароль!")]
    public string Password { get; set; } = default!;
    // [Required]
    [EmailAddress (ErrorMessage = "Вы ввели некорректный Email")]
    public string Email { get; set; } = default!;
    // [Required (ErrorMessage = "Укажите свое имя!")]
    public string FirstName { get; set; } = default!;
    // [Required (ErrorMessage = "Укажите свою фамилию!")]
    public string LastName { get; set; } = default!;
}