using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebKursovaya.Models
{
    public class Экзамены
    {
        [Key]
        public int Код_экзамена { get; set; }

        [ForeignKey("Заявитель")]
        public int Код_заявителя { get; set; }

        [Display(Name = "Дата сдачи")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле 'Дата сдачи' обязательно для заполнения")]
        public DateTime Дата_сдачи { get; set; }

        [ForeignKey("Сотрудник")]
        public int Код_сотрудника { get; set; }

        [ForeignKey("Категория")]
        public int Код_категории_ТС { get; set; }
        

        [Display(Name = "Результат сдачи")]
        [Required(ErrorMessage = "Поле 'Результат сдачи' обязательно для заполнения")]
        public string Результат_сдачи { get; set; }

        public Заявители? Заявитель { get; set; }
        public Сотрудники? Сотрудник { get; set; }
        public Категории_ТС? Категория { get; set; }

        // Свойство для вывода ФИО
        [NotMapped]
        public string СотрудникФИО => $"{Сотрудник?.Фамилия} {Сотрудник?.Имя} {Сотрудник?.Отчество}";
        [NotMapped]
        public string ЗаявительФИО => $"{Заявитель?.Фамилия} {Заявитель?.Имя} {Заявитель?.Отчество}";

    }

}
