using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebKursovaya.Models
{
    public class Виды_статусов_ВУ
    {
        [Key]
        public int Код_статуса_ВУ { get; set; }

        [Display(Name = "Название статуса")]
        [Required(ErrorMessage = "Поле 'Название статуса' обязательно для заполнения")]
        public string Название_статуса { get; set; }
    }
}
