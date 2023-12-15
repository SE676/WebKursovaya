using System.ComponentModel.DataAnnotations;
namespace WebKursovaya.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле 'Логин' обязательно для заполнения")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
