using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebKursovaya.Models
{
    public class Посещения
    {
        [Key]
        public int Код_посещения {  get; set; }
        [Required(ErrorMessage = "Поле 'Фамилия' обязательно для заполнения")]
        public string Фамилия { get; set; }
        [Required(ErrorMessage = "Поле 'Имя' обязательно для заполнения")]
        public string Имя { get; set; }
        [Required(ErrorMessage = "Поле 'Отчество' обязательно для заполнения")]
        public string Отчество { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] 
        public DateTime Дата_рождения { get; set; }


        [Required(ErrorMessage = "Поле 'Дата посещения' обязательно для заполнения")]
        [Display(Name = "Дата посещения")]
        [FutureDateTimeValidation(ErrorMessage = "Допустима только дата посещения, начиная с сегодняшнего дня и позднее, при условии, что время посещения находится в промежутке от 08:00 до 18:00 и с интервалом каждые 15 минут (00, 15, 30, 45).")]
        public DateTime Дата_посещения { get; set; }
    }
}
