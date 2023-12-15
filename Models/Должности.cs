using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebKursovaya.Models
{
    public class Должности
    {
        [Key]
        public int Код_должности { get; set; }
        [Display(Name = "Название должности")]
        [Required(ErrorMessage = "Поле 'Название должности' обязательно для заполнения")]
        public string Название_должности { get; set; }

    }
}