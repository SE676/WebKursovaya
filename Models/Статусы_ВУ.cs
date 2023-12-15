using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace WebKursovaya.Models
{
    public class Статусы_ВУ
    {
        [Key]
        [Column(Order = 0)]
        public DateTime Дата {  get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("ВУ")]
        public int Код_ВУ { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("Заявитель")]
        public int Код_заявителя {  get; set; }

        [ForeignKey("Вид_статуса_ВУ")]
        public int Код_статуса_ВУ {  get; set; }

        public virtual ВУ ВУ {  get; set; }
        public virtual Заявители Заявитель { get; set; }
        public virtual Виды_статусов_ВУ Вид_статуса_ВУ { get; set; }
        }
}
