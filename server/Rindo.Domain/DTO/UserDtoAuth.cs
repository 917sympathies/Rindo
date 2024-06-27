using System.ComponentModel.DataAnnotations;

namespace Rindo.Domain.DTO;

public class UserDtoAuth
{
    [Required(ErrorMessage = "Необходимо указать ваше имя пользователя!")]
    public string Username { get; set; } = default!;
    [Required(ErrorMessage = "Необходимо указать пароль!")]
    public string Password { get; set; } = default!;
}