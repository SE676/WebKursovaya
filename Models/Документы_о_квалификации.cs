using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace WebKursovaya.Models
{
    public class Документы_о_квалификации
    {
        [Key]
        public int Номер_документа_о_квалификации { get; set; }

        
        [ForeignKey("Заявитель")]
        public int Код_заявителя { get; set; }


        [ForeignKey("Автошкола")]
        public int Код_автошколы { get; set; }

        [ForeignKey("Категория")]
        public int Код_категории_ТС { get; set; }

        [Display(Name = "Дата выдачи")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле 'Дата выдачи' обязательно для заполнения")]
        
        public DateTime Дата_выдачи {  get; set; }

        public Заявители? Заявитель { get; set; }
        public Автошколы? Автошкола { get; set; }
        public Категории_ТС? Категория { get; set; }


        // Свойство для вывода ФИО
        [NotMapped]
        public string ЗаявительФИО => $"{Заявитель?.Фамилия} {Заявитель?.Имя} {Заявитель?.Отчество}";

    }
}
