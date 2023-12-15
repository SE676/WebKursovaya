using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebKursovaya.Models
{
    public class Заявления
    {
        [Key]
        public int Номер_заявления { get; set; }

        [ForeignKey("Виды_статусов_заявлений")]
        public int Код_статуса_заявления { get; set; }

        [ForeignKey("Заявитель")]
        public int Код_заявителя { get; set; }

        [ForeignKey("Вид_услуги")]
        public int Код_вида_услуги { get; set; }

        public Заявители? Заявитель { get; set; }

        [Display(Name = "Вид услуги")]
        public Виды_услуг? Вид_услуги { get; set; }

        [Display(Name = "Статус заявления")]
        public Виды_статусов_заявлений? Виды_статусов_заявлений { get; set; }

        [NotMapped]
        public string ЗаявительФИО => $"{Заявитель?.Фамилия} {Заявитель?.Имя} {Заявитель?.Отчество}";


    }
}
