using System.ComponentModel.DataAnnotations;
namespace WebKursovaya.Models
{
    public class Мед_организации
    {
        [Key]
        public int Код_медицинской_организации { get; set; }
        [Display(Name = "Название мед. организации")]
        [Required(ErrorMessage = "Поле 'Название мед. организации' обязательно для заполнения")]
        public string Название_медицинской_организации { get;set; }
        [Display(Name = "Адрес мед. организации")]
        [Required(ErrorMessage = "Поле 'Адрес мед. организации' обязательно для заполнения")]
        public string Адрес_медицинской_организации { get; set; }
        [Display(Name = "Email мед. организации")]
        [Required(ErrorMessage = "Поле 'Email мед. организации' обязательно для заполнения")]
        public string Email_медицинской_организации { get; set; }
    }
}
