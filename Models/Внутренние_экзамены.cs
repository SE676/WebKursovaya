using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;



namespace WebKursovaya.Models
{
    public class Внутренние_экзамены
    {
        [Key]
        public int Код_внутреннего_экзамена { get; set; }
        [ForeignKey("Автошкола")]
        public int Код_автошколы { get; set; }
        [ForeignKey("Заявитель")]
        public int Код_заявителя { get; set; }

        [Display(Name = "Дата сдачи")]
        public DateTime Дата_внутреннего_экзамена{ get; set; }

        [Display(Name = "Результат сдачи")]
        public string Результат_внутреннего_экзамена { get; set; }

        public Автошколы? Автошкола { get; set; }

        public Заявители? Заявитель { get; set; }

        [NotMapped]
        public string АШ => $"{Автошкола?.Название_автошколы}";

        [NotMapped]
        public string ЗаявительФИО => $"{Заявитель?.Фамилия} {Заявитель?.Имя} {Заявитель?.Отчество}";
    }

}

