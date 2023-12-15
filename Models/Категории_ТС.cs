using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebKursovaya.Models
{
    public class Категории_ТС
    {
        [Key]
        public int Код_категории_ТС { get; set; }
        [Display(Name = "Наименование категории ТС")]
        [Required(ErrorMessage = "Поле 'Наименование категории ТС' обязательно для заполнения")]
        public string? Наименование_категории_ТС { get; set; }

    }
}
