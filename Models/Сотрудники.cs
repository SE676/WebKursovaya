using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace WebKursovaya.Models
{
    public class Сотрудники
    {
        [Key]
        public int Код_сотрудника { get; set; }
        [Required(ErrorMessage = "Поле 'Фамилия' обязательно для заполнения")]
        public string Фамилия { get; set; }
        [Required(ErrorMessage = "Поле 'Имя' обязательно для заполнения")]
        public string Имя { get; set; }
        [Required(ErrorMessage = "Поле 'Отчество' обязательно для заполнения")]
        public string Отчество { get; set; }

        [ForeignKey("Должность")]
        public int Код_должности { get; set; }
        
        public Должности? Должность { get; set; }

        [NotMapped]
        public string СотрудникФИО => $"{Фамилия} {Имя} {Отчество}";

    }
}
