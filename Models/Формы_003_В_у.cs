using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebKursovaya.Models
{
    public class Формы_003_В_у
    {
        [Key]
        public int Номер_формы_003_В_у {  get; set; }

        [ForeignKey("Заявитель")]
        public int Код_заявителя { get; set; }

        [ForeignKey("Мед_организация")]
        public int Код_медицинской_организации { get; set; }

        [ForeignKey("Категория")]
        public int Код_категории_ТС { get; set; }
        
        [Display(Name = "Дата выдачи")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле 'Дата выдачи' обязательно для заполнения")]
        
        public DateTime Дата_выдачи { get; set; }

        [Display(Name = "Cведения о медицинских противопоказаниях")]
        [Required(ErrorMessage = "Поле 'Cведения о медицинских противопоказаниях' обязательно для заполнения")]
        public string Сведения_о_медицинских_противопоказаниях {  get; set; }

        public Заявители? Заявитель { get; set; }

        [Display(Name = "Мед. организация")]
        public Мед_организации? Мед_организация { get; set; }
        public Категории_ТС? Категория { get; set; }

        [NotMapped]
        public string ЗаявительФИО => $"{Заявитель?.Фамилия} {Заявитель?.Имя} {Заявитель?.Отчество}";

    }
}
