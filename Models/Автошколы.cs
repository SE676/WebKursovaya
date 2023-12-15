using System.ComponentModel.DataAnnotations;
namespace WebKursovaya.Models
{
    public class Автошколы
    {
        [Key]
        [Display(Name = "Код автошколы")]
        
        public int Код_автошколы { get; set; }
        [Display(Name = "Название автошколы")]
        [Required(ErrorMessage = "Поле 'Название автошколы' обязательно для заполнения")]
        public string Название_автошколы { get; set; }
        [Display(Name = "Адрес автошколы")]
        [Required(ErrorMessage = "Поле 'Адрес автошколы' обязательно для заполнения")]
        public string Адрес_автошколы { get; set; }
        [Display(Name = "Email автошколы")]
        [Required(ErrorMessage = "Поле 'Email автошколы' обязательно для заполнения")]
        public string Email_автошколы { get; set; }
    }
}
