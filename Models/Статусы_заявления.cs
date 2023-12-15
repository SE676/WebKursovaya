using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebKursovaya.Models
{
    public class Статусы_заявления
    {
        [Key]
        [Column(Order = 0)]
        public DateTime Дата { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Заявление")]
        public int Номер_Заявления { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("Заявитель")]
        public int Код_заявителя { get; set; }

        [ForeignKey("Вид_статуса_заявления")]
        public int Код_статуса_заявления { get; set; }

        public virtual Заявления Заявление { get; set; }
        public virtual Заявители Заявитель { get; set; }
        public virtual Виды_статусов_заявлений Вид_статуса_заявления { get; set; }
    }
}

