using System.ComponentModel.DataAnnotations;
namespace WebKursovaya.Models
{
    public class Виды_статусов_заявлений
    {
        [Key]
        public int Код_статуса_заявления { get; set; }
        [Display(Name = "Название статуса")]
        [Required(ErrorMessage = "Поле 'Название статуса' обязательно для заполнения")]
        public string Название_статуса { get; set; }
    }
}
