using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebKursovaya.Models
{
    public class Заявители
    {
        [Key]
        public int Код_заявителя { get; set; }
        [Required(ErrorMessage = "Поле 'Фамилия' обязательно для заполнения")]
        public string Фамилия { get; set; }
        [Required(ErrorMessage = "Поле 'Имя' обязательно для заполнения")]
        public string Имя { get; set; }
        [Required(ErrorMessage = "Поле 'Отчество' обязательно для заполнения")]
        public string Отчество { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле 'Дата рождения' обязательно для заполнения")]
        
        public DateTime Дата_рождения { get; set; }

        [ForeignKey("Пол")]
        public int Код_пола { get; set; }

        public Полы? Пол { get; set; }

        [NotMapped]
        public string ЗаявительФИО => $"{Фамилия} {Имя} {Отчество}";

        
    }
}
