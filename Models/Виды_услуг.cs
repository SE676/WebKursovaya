using System.ComponentModel.DataAnnotations;
namespace WebKursovaya.Models
{
    public class Виды_услуг
    {
        [Key]
        public int Код_вида_услуги { get; set; }

        [Display(Name = "Название вида услуги")]
        [Required(ErrorMessage = "Поле 'Название вида услуги' обязательно для заполнения")]
        public string Название_вида_услуги { get; set; }
    }
}
