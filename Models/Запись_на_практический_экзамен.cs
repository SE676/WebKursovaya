using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebKursovaya.Models
{
    public class Запись_на_практический_экзамен
    {
        [Key]
        public int Код_записи {  get; set; }

        [Display(Name = "Дата записи")]
        [Required(ErrorMessage = "Поле 'Дата записи' обязательно для заполнения")]
       
        public DateTime Дата_записи { get; set; }


        [ForeignKey("Заявитель")]
        public int Код_заявителя { get; set; }


        [ForeignKey("Категория")]
        public int Код_категории_ТС { get; set; }

        public Заявители? Заявитель { get; set; }
        public Категории_ТС? Категория { get; set; }

        [NotMapped]
        public string ЗаявительФИО => $"{Заявитель?.Фамилия} {Заявитель?.Имя} {Заявитель?.Отчество}";


    }
}
