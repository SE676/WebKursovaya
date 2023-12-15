using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebKursovaya.Models
{
    public class Справки_уплаты_госпошлины
    {
        [Key]
        public int Номер_справки_уплаты_госпошлины { get; set; }

        [ForeignKey("Заявитель")]
        public int Код_заявителя {  get; set; }

        //[Display(Name = "Cведения об уплате")]
        //public string Сведения_об_уплате {  get; set; }

        [ForeignKey("Дата")]
        public int Код_стоимости {  get; set; }


       
        public Заявители? Заявитель { get; set; }

        [Display(Name = "Дата")]
        public Стоимость_госпошлины? Дата {  get; set; }

        [NotMapped]
        public string ЗаявительФИО => $"{Заявитель?.Фамилия} {Заявитель?.Имя} {Заявитель?.Отчество}";

    }
}
